using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Recruiter : User
    {
        public Company Company { get; set; } = new Company();
        public int CompanyId { get; set; } = 0;
    }
}
