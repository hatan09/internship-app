using InternshipApp.Core.Entities;

namespace InternshipApp.Repository;

public interface IMatchingService
{
    #region [ Public Methods - Ranking ]
    public Task<int> GetMatchingPointById(string studentId, int jobId);
    public int GetMatchingPoint(List<StudentSkill> studentSkills, List<JobSkill> jobSkills, List<SkillScore> skillScores);
    #endregion
}
