namespace InternshipApp.Core.Entities
{
    public class Skill : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;

        public SkillType Type { get; set; } = SkillType.Other;

        public virtual ICollection<StudentSkill>? StudentSkills { get; set; } = new HashSet<StudentSkill>();
        public virtual ICollection<JobSkill>? JobSkills { get; set; } = new HashSet<JobSkill>();
    }

    public enum SkillType
    {
        Other, Language, Field, Role, 
    }

    public enum SkillLevel
    {
        NOVICE, LITTLE, AVERAGE, PROFICIENCY
    }
}
