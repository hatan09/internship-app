using InternshipApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipApp.Contracts
{
    public interface ISkillRepository : IBaseRepository<Skill>
    {
        public IQueryable<InternGroup> FindByDepartment(int id);
    }
}
