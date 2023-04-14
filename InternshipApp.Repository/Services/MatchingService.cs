using InternshipApp.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Repository;

public class MatchingService : IMatchingService
{
    #region [ Fields ]
    private readonly StudentManager _studentManager;
    private readonly IJobRepository _jobRepository;
    #endregion

    #region [ CTor ]
    public MatchingService(StudentManager studentManager, IJobRepository jobRepository)
    {
        _studentManager = studentManager;
        _jobRepository = jobRepository;

        ScoreMap = new()
        {
            {
                1,
                new()
                {
                    { 1, MatchingType.FIT },
                    { 2, MatchingType.FIT },
                    { 3, MatchingType.NEARLYFIT },
                }
            }
        };
    }
    #endregion

    public async Task<int> GetMatchingPoint(string studentId, int jobId)
    {
        int score = 0;
        double k_factor = 0;

        var student = await _studentManager.FindAll(x => x.Id == studentId).Include(x => x.StudentSkills).FirstOrDefaultAsync();
        var studentSkills = student?.StudentSkills;
        if (studentSkills == null || studentSkills.Count == 0) return 0;

        var job = await _jobRepository.FindAll(x => x.Id == jobId).Include(x => x.JobSkills).FirstOrDefaultAsync();
        var jobSkills = job?.JobSkills;
        if (jobSkills == null || jobSkills.Count == 0) return 100;

        if (jobSkills.Where(x => x.Weight <= 0).Any())
            k_factor = (1 - jobSkills.Sum(x => x.Weight)) /
                        jobSkills.Where(x => x.Weight <= 0).Count();

        foreach (var jobSkill in jobSkills)
        {
            if(jobSkill.Weight <= 0)    // additional soft skills
            {
                if (studentSkills.Where(x => x.SkillId == jobSkill.SkillId).Any())
                    score += (int)(100 * k_factor);
                continue;
            }

            // primary skills
            if (studentSkills.Where(x => x.SkillId == jobSkill.SkillId).Any())
                score += (int)(100 * jobSkill.Weight);
            else
            {
                var skillScore = ScoreMap[jobSkill.SkillId];
                if (skillScore == null)
                    continue;

                int max = 0;
                foreach (var studentSkill in studentSkills)
                {
                    if (!skillScore.TryGetValue((int)studentSkill.SkillId, out var matchingType))
                    {
                        continue;
                    }

                    max = Math.Max(max, (int) (100 * jobSkill.Weight * GetSkillMatchPoint(matchingType)));
                }
                score += max;
            }
        }

        return score;
    }

    private double GetSkillMatchPoint(MatchingType type)
    {
        switch (type)
        {
            case MatchingType.FIT:
                {
                    return 1;
                }
            case MatchingType.NEARLYFIT:
                {
                    return 0.7;
                }
            case MatchingType.AVERAGE:
                {
                    return 0.5;
                }
            default:
                {
                    return 0;
                }
        }
    }

    private Dictionary<int, Dictionary<int, MatchingType>> ScoreMap { get; set; }

    public enum MatchingType { FIT, NEARLYFIT, AVERAGE }
}
