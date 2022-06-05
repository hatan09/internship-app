using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApp.Api.DataObjects
{
    public class InternGroupDTO : BaseDTO<int>
    {
        public int DepartmentId { get; set; } = 0;

        public string InstructorId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int Slots { get; set; } = 0;
    }
}
