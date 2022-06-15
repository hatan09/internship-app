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

        public virtual Instructor? Instructor { get; set; }
        public virtual ICollection<Job?> Jobs { get; set; } = new HashSet<Job?>();
    }
}
