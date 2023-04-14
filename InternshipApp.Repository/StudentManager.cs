using System.Linq.Expressions;
using InternshipApp.Core.Entities;
using InternshipApp.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InternshipApp.Repository
{
    public class StudentManager : UserManager<Student>
    {
        public StudentManager(
            IUserStore<Student> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Student> passwordHasher,
            IEnumerable<IUserValidator<Student>> userValidators,
            IEnumerable<IPasswordValidator<Student>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Student>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

        public new async Task<Student?> FindByNameAsync(string userName)
        {
            var student = await base.FindByNameAsync(userName);
            if (student is null || student.IsDeleted)
                return null;

            return student;
        }

        public IQueryable<Student> FindAll(Expression<Func<Student, bool>>? predicate = null)
            => Users
                .Where(s => !s.IsDeleted)
                .WhereIf(predicate != null, predicate!);

        public async Task<Student?> FindByStudentId(string studentId, CancellationToken cancellationToken)
            => await FindAll().Where(stu => stu.StudentId.Equals(studentId)).FirstOrDefaultAsync(cancellationToken);
    }
}
