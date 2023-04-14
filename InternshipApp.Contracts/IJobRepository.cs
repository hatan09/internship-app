using InternshipApp.Core.Entities;

namespace InternshipApp.Contracts;

public interface IJobRepository : IBaseRepository<Job>
{
    IQueryable<Job> FindByCompanyId(int companyId);
}
