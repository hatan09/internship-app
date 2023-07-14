using System.Linq.Expressions;
using InternshipApp.Core.Entities;
using InternshipApp.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public new async Task<User?> FindBySignalRConnectionId(string connectionId)
        {
            return await Users.Where(x => !x.IsDeleted && x.SignalRConnectionId == connectionId).Include(x => x.Conversations).FirstOrDefaultAsync();
        }

        public IQueryable<User> FindAll(Expression<Func<User, bool>>? predicate = null)
            => Users
                .Where(s => !s.IsDeleted)
                .WhereIf(predicate != null, predicate!);
    }
}
