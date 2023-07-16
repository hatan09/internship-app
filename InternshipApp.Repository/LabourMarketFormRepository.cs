using System.Linq.Expressions;
using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using InternshipApp.Repository.Extensions;

namespace InternshipApp.Repository
{
    public class LabourMarketFormRepository : BaseRepository<LabourMarketForm>, ILabourMarketFormRepository
    {
        public LabourMarketFormRepository(AppDbContext context) : base(context) { }

        public override IQueryable<LabourMarketForm> FindAll(Expression<Func<LabourMarketForm, bool>>? predicate = null)
            => _dbSet.WhereIf(predicate != null, predicate!);
    }
}
