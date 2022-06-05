using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using InternshipApp.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipApp.Repository
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(AppDbContext context) : base(context) { }

        public override IQueryable<Skill> FindAll(Expression<Func<Skill, bool>>? predicate = null)
            => _dbSet.WhereIf(predicate != null, predicate!);

        public IQueryable<InternGroup> FindByDepartment(int id)
        {
            throw new NotImplementedException();
        }
    }
}
