using Microsoft.AspNetCore.Identity;

namespace InternshipApp.Core.Entities
{
    public class StudentSkill
    {
        public Student? Student { get; set; }
        public string? StudentId { get; set; } = string.Empty;
        public Skill? Skill { get; set; }
        public int? SkillId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
