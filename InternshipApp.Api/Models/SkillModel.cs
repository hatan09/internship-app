using InternshipApp.Core.Entities;

namespace InternshipApp.Api.Models
{
    public class AddSkillModel
    {
        public string StudentId { get; set; } = string.Empty;
        public ICollection<int> Skills { get; set; } = new List<int>();
    }

    public class DeleteSkillModel
    {
        public string StudentId { get; set; } = string.Empty;
        public ICollection<int> Skills { get; set; } = new List<int>();
    }

    public class UpdateSkillModel
    {
        public string StudentId { get; set; } = string.Empty;
        public ICollection<StudentSkill> Skills { get; set; } = new List<StudentSkill>();
    }
}
