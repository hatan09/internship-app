using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApp.Api.DataObjects
{
    public class SkillDTO : BaseDTO<int>
    {
        public string Name { get; set; } = string.Empty;
    }
}
