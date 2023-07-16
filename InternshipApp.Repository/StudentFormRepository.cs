using System.Linq.Expressions;
using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using InternshipApp.Repository.Extensions;

namespace InternshipApp.Repository
{
    public class StudentFormRepository : BaseRepository<StudentForm>, IStudentFormRepository
    {
        public StudentFormRepository(AppDbContext context) : base(context) { }

        public override IQueryable<StudentForm> FindAll(Expression<Func<StudentForm, bool>>? predicate = null)
            => _dbSet.WhereIf(predicate != null, predicate!);
    }
}
