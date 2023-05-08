using InternshipApp.Core.Entities;

namespace InternshipApp.Contracts;

public interface ISkillScoreRepository : IBaseRepository<SkillScore>
{
    public Task<List<SkillScore>> FindBySkillAsync(int skillId);
}
