using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApp.Api.DataObjects
{
    public class StudentDTO : BaseDTO<string>
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public string Birthdate { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string StudentId { get; set; } = string.Empty;

        public int Credit { get; set; } = 0;

        public double GPA { get; set; } = 0;

        public Stat Stat { get; set; } = Stat.PENDING;

        public string CVUrl { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public int? DepartmentId { get; set; }
        public int? InternGroupId { get; set; }
    }

    public class GetStudentDTO : BaseDTO<string>
    {
        public string FullName { get; set; } = string.Empty;

        public string StudentId { get; set; } = string.Empty;

        public int Credit { get; set; } = 0;

        public double GPA { get; set; } = 0;

        public Stat Stat { get; set; } = Stat.PENDING;

        public bool IsAccepted { get; set; } = false;

        public string CVUrl { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public string Birthdate { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<int> SkillIds { get; set; } = Array.Empty<int>();
    }
}
