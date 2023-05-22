using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class InternGroup : BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public int Slots { get; set; }

        public virtual Instructor? Instructor { get; set; }
        public string? InstructorId { get; set; }

        public virtual Department? Department { get; set; }
        public int? DepartmentId { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
