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
    public class UserManager : UserManager<User>
    {
        public UserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }

        public new async Task<User?> FindByNameAsync(string userName)
        {
            var student = await base.FindByNameAsync(userName);
            if (student is null || student.IsDeleted)
                return null;

            return student;
        }

        public IQueryable<User> FindAll(Expression<Func<User, bool>>? predicate = null)
            => Users
                .Where(s => !s.IsDeleted)
                .WhereIf(predicate != null, predicate!);
    }
}
