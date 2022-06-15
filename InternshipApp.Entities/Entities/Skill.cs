using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Skill : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<StudentSkill>? StudentSkills { get; set; } = new HashSet<StudentSkill>();
        public virtual ICollection<JobSkill>? JobSkills { get; set; } = new HashSet<JobSkill>();
    }

    public enum Level { BEGINNER, INTERMEDIATE, ADVANCE, MASTER }
}
