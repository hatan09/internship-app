namespace InternshipApp.Core.Entities
{
    public class Skill : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;

        public SkillType Type { get; set; } = SkillType.OTHER;

        public virtual ICollection<StudentSkill>? StudentSkills { get; set; } = new HashSet<StudentSkill>();
        public virtual ICollection<JobSkill>? JobSkills { get; set; } = new HashSet<JobSkill>();
    }

    public enum SkillType
    {
        OTHER, CONCEPT, TECH, FEILD, ROLE, 
    }

    public enum SkillLevel
    {
        NOVICE, LITTLE, AVERAGE, PROFICIENCY
    }
}
