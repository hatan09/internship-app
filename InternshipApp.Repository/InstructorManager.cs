using InternshipApp.Core.Entities;
using InternshipApp.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipApp.Repository
{
    public class InstructorManager : UserManager<Instructor>
    {
        public InstructorManager(
            IUserStore<Instructor> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Instructor> passwordHasher,
            IEnumerable<IUserValidator<Instructor>> userValidators,
            IEnumerable<IPasswordValidator<Instructor>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Instructor>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

        public new async Task<Instructor?> FindByNameAsync(string userName)
        {
            var instructor = await base.FindByNameAsync(userName);
            if (instructor is null || instructor.IsDeleted)
                return null;

            return instructor;
        }

        public IQueryable<Instructor> FindAll(Expression<Func<Instructor, bool>>? predicate = null)
            => Users
                .Where(s => !s.IsDeleted)
                .WhereIf(predicate != null, predicate!);
    }
}
