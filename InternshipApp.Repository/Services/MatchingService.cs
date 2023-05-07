using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
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
            },
            {
                2,
                new()
                {
                    
                }
            },
            {
                3,
                new()
                {
                    
                }
            },
            {
                4,
                new()
                {

                }
            },
            {
                5,
                new()
                {

                }
            },
            {
                6,
                new()
                {

                }
            },
            {
                7,
                new()
                {

                }
            },
            {
                8,
                new()
                {

                }
            },
            {
                9,
                new()
                {

                }
            },
            {
                10,
                new()
                {

                }
            },
            {
                11,
                new()
                {

                }
            },
            {
                12,
                new()
                {

                }
            },
            {
                13,
                new()
                {

                }
            },
            {
                14,
                new()
                {

                }
            },
            {
                15,
                new()
                {

                }
            },
            {
                16,
                new()
                {

                }
            },
            {
                17,
                new()
                {

                }
            },
            {
                18,
                new()
                {

                }
            },
            {
                19,
                new()
                {

                }
            },
            {
                20,
                new()
                {

                }
            },
            {
                21,
                new()
                {

                }
            },
            {
                22,
                new()
                {

                }
            },
            {
                23,
                new()
                {

                }
            },
            {
                24,
                new()
                {

                }
            },
            {
                25,
                new()
                {

                }
            },
            {
                26,
                new()
                {

                }
            },
            {
                27,
                new()
                {

                }
            },
            {
                28,
                new()
                {

                }
            },
            {
                29,
                new()
                {

                }
            },
            {
                30,
                new()
                {

                }
            },
            {
                31,
                new()
                {

                }
            },
            {
                32,
                new()
                {

                }
            },
            {
                33,
                new()
                {

                }
            },
            {
                34,
                new()
                {

                }
            },
            {
                35,
                new()
                {

                }
            },
            {
                36,
                new()
                {

                }
            },
            {
                37,
                new()
                {

                }
            },
            {
                38,
                new()
                {

                }
            },
            {
                39,
                new()
                {

                }
            },
            {
                40,
                new()
                {

                }
            },
            {
                41,
                new()
                {

                }
            },
            {
                42,
                new()
                {

                }
            },
            {
                43,
                new()
                {

                }
            },
            {
                44,
                new()
                {

                }
            },
            {
                45,
                new()
                {

                }
            },

        };
    }
    #endregion

    public async Task<int> GetMatchingPointById(string studentId, int jobId)
    {
        int score = 0;
        double k_factor = 0;

        var student = await _studentManager.FindAll(x => x.Id == studentId).Include(x => x.StudentSkills).FirstOrDefaultAsync();
        var studentSkills = student?.StudentSkills;
        if (studentSkills == null || studentSkills.Count == 0) return 0;    // student has no skill? => no points

        var job = await _jobRepository.FindAll(x => x.Id == jobId).Include(x => x.JobSkills).FirstOrDefaultAsync();
        var jobSkills = job?.JobSkills;
        if (jobSkills == null || jobSkills.Count == 0) return 100;  // job has no skill? => free points

        if (jobSkills.Where(x => x.Weight <= 0).Any())
        {
            // k_factor is used to find score of skill with weight 0 when job's total weight does not sum up to 1
            k_factor = (1 - jobSkills.Sum(x => x.Weight)) /
                        jobSkills.Where(x => x.Weight <= 0).Count();
        }
        else if(jobSkills.Sum(x => x.Weight) < 1)
        {
            // no soft skills => all job's weight that missing will be free!
            score += (int) (100 * (1 - jobSkills.Sum(x => x.Weight)));
        }
            
        // loop through all job's skills, for each of them => find student's match skill or best alternative skill
        foreach (var jobSkill in jobSkills)
        {
            if (jobSkill.Weight <= 0)    // additional soft skills => multiply by k_factor
            {
                if (studentSkills.Where(x => x.SkillId == jobSkill.SkillId).Any())
                    score += (int)(100 * k_factor);
                continue;
            }

            // primary skills
            if (studentSkills.FirstOrDefault(x => x.SkillId == jobSkill.SkillId) != null)  // match exact required skill
                score += (int)(100 * jobSkill.Weight);
            else    // find alternative skills
            {
                var skillScore = ScoreMap[jobSkill.SkillId];    // get all alternatives
                if (skillScore == null) // ==> no alternative
                    continue;

                int max = 0;
                foreach (var studentSkill in studentSkills) // find all student's skills that can be alternative for that job's skill
                {
                    if (!skillScore.TryGetValue((int)studentSkill.SkillId, out var matchingType))   // ==> not alternative, next
                    {
                        continue;
                    }

                    // ==> alternative, get points. alternative with highest point (best matched) will be taken to add to matching points
                    max = Math.Max(max, (int)(100 * jobSkill.Weight * GetSkillMatchPoint(matchingType)));
                    if (matchingType == MatchingType.FIT) break;    // ==> if a skill is marked as fit => consider it as 100% matched skill. stop
                }
                score += max;   // add score of alternative skill
            }
        }

        return score > 100 ? 100 : score;
    }

    public int GetMatchingPoint(List<StudentSkill> studentSkills, List<JobSkill> jobSkills)
    {
        int score = 0;
        double k_factor = 0;
        if (studentSkills == null || studentSkills.Count == 0) return 0;
        if (jobSkills == null || jobSkills.Count == 0) return 100;

        if (jobSkills.Where(x => x.Weight <= 0).Any())
            k_factor = (1 - jobSkills.Sum(x => x.Weight)) /
                        jobSkills.Where(x => x.Weight <= 0).Count();

        foreach (var jobSkill in jobSkills)
        {
            if (jobSkill.Weight <= 0)    // additional soft skills
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

                    max = Math.Max(max, (int)(100 * jobSkill.Weight * GetSkillMatchPoint(matchingType)));
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

    
}
