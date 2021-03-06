using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class Student : User
    {
        public string StudentId { get; set; } = string.Empty;
        public float GPA { get; set; }
        public Stat Stat { get; set; } = Stat.PENDING;
        public int Credit { get; set; }
        public string CVUrl { get; set; } = string.Empty;

        //[Required]
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
        //[Required]
        public InternGroup? InternGroup { get; set; }
        public int? InternGroupId { get; set; }

        public virtual ICollection<StudentSkill>? StudentSkills { set; get; } = new HashSet<StudentSkill>();
        public virtual ICollection<StudentJob>? StudentJobs { set; get; } = new HashSet<StudentJob>();


    }

    public enum Stat { PENDING, ACCEPTED, DENIED}

}
