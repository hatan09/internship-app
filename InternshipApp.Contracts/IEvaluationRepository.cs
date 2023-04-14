using InternshipApp.Core.Entities;

namespace InternshipApp.Contracts;

public interface IEvaluationRepository : IBaseRepository<Evaluation>
{
    public Task<Evaluation?> FindByStudentAsync(string studentId);
}
