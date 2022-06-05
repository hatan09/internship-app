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
    public class RecruiterManager : UserManager<Recruiter>
    {
        public RecruiterManager(
            IUserStore<Recruiter> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Recruiter> passwordHasher,
            IEnumerable<IUserValidator<Recruiter>> userValidators,
            IEnumerable<IPasswordValidator<Recruiter>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Recruiter>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

        public new async Task<Recruiter?> FindByNameAsync(string userName)
        {
            var recruiter = await base.FindByNameAsync(userName);
            if (recruiter is null || recruiter.IsDeleted)
                return null;

            return recruiter;
        }

        public IQueryable<Recruiter> FindAll(Expression<Func<Recruiter, bool>>? predicate = null)
            => Users
                .Where(s => !s.IsDeleted)
                .WhereIf(predicate != null, predicate!);
    }
}
