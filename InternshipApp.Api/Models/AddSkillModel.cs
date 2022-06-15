namespace InternshipApp.Api.Models
{
    public class AddSkillModel
    {
        public string StudentId { get; set; } = string.Empty;
        public ICollection<int> Skills { get; set; } = new List<int>();
    }
}
