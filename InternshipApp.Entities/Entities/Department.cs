using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Department : BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;

        public virtual ICollection<InstructorDepartment> InstructorDepartments { get; set; } = new HashSet<InstructorDepartment>();
        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}
