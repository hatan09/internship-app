using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Instructor : User
    {
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        public virtual InternGroup? InternGroup { get; set; }
    }
}
