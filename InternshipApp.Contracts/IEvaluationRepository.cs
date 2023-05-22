using InternshipApp.Core.Entities;

namespace InternshipApp.Contracts;

public interface IEvaluationRepository : IBaseRepository<Evaluation>
{
    public Task<List<Evaluation>> FindByStudentAsync(string studentId);
}
