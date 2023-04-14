using InternshipApp.Core.Entities;

namespace InternshipApp.Contracts;

public interface IInternGroupRepository : IBaseRepository<InternGroup>
{
    public IQueryable<InternGroup> FindByDepartment(int id);
}
