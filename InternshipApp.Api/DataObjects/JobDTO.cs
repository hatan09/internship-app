using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApp.Api.DataObjects
{
    public class JobDTO : BaseDTO<int>
    {
        public int MinCredit { get; set; } = 0;

        public double MinGPA { get; set; } = 0;

        public int Slots { get; set; } = 0;

        public string Title { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public ICollection<int> SkillIds { get; set; } = Array.Empty<int>();
    }

    public class CreateJobDTO
    {
        public int MinCredit { get; set; } = 0;

        public double MinGPA { get; set; } = 0;

        public int Slots { get; set; } = 0;

        public string Title { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public ICollection<int> DepartmentIds { get; set; } = Array.Empty<int>();

        public ICollection<int> SkillIds { get; set; } = Array.Empty<int>();
    }
}
