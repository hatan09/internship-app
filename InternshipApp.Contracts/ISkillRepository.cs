using InternshipApp.Core.Entities;

namespace InternshipApp.Contracts;

public interface ISkillRepository : IBaseRepository<Skill>
{
    public IQueryable<InternGroup> FindByDepartment(int id);
}
