using Microsoft.AspNetCore.Identity;

namespace InternshipApp.Core.Entities
{
    public class StudentJob
    {
        public bool IsAccepted { get; set; }
        public Student? Student { get; set; }
        public string? StudentId { get; set; } = string.Empty;
        public Job? Job { get; set; }
        public int? JobId { get; set; }
    }
}
