using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Job : BaseEntity<int>
    {
        public int MinCredit { get; set; }
        public double MinGPA { get; set; }
        public int Slots { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MinYear { get; set; }
        public string? Description { get; set; }
        public string? JobPaths { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public virtual ICollection<Department>? Departments { get; set; } = new HashSet<Department>();

        public virtual ICollection<StudentJob>? StudentJobs { get; set; } = new HashSet<StudentJob>();
        public virtual ICollection<JobSkill>? JobSkills { get; set; } = new HashSet<JobSkill>();
    }
}
