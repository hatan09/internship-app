using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Company : BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string? CompanyWebsite { get; set; }
        public string Address { get; set; } = string.Empty;

        public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}
