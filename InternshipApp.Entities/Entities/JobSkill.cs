using Microsoft.AspNetCore.Identity;

namespace InternshipApp.Core.Entities
{
    public class JobSkill
    {
        public Job Job { get; set; }
        public int JobId { get; set; }
        public Skill Skill { get; set; }
        public int SkillId { get; set; }
        public Level? Level { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}
