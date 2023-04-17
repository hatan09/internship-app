using System.ComponentModel.DataAnnotations;

namespace InternshipApp.Core.Entities
{
    public class JobSkill
    {
        public Job Job { get; set; }
        public int JobId { get; set; }
        public Skill Skill { get; set; }
        public int SkillId { get; set; }
        public SkillLevel Level { get; set; } = SkillLevel.NOVICE;
        [Range(0, 1)]
        public double Weight { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
    }
}
