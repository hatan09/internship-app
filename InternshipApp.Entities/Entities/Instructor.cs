using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Instructor : User
    {
        public virtual ICollection<InstructorDepartment> InstructorDepartments { get; set; } = new HashSet<InstructorDepartment>();
    }
}
