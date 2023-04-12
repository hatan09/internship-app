using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Repository;

public class EvaluationRepository : BaseRepository<Evaluation>, IEvaluationRepository
{
    public EvaluationRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Evaluation?> FindByStudentAsync(string studentId)
        => FindAll(x => x.StudentId == studentId).FirstOrDefaultAsync();
}
