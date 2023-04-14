using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace InternshipApp.Core.Entities
{
    public class JobSkill
    {
        public Job Job { get; set; }
        public int JobId { get; set; }
        public Skill Skill { get; set; }
        public int SkillId { get; set; }
        [Range(0, 1)]
        public double Weight { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
    }
}
