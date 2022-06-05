using Microsoft.AspNetCore.Identity;

namespace InternshipApp.Core.Entities
{
    public class InstructorDepartment
    {
        public string? Position { get; set; } = string.Empty;
        public Instructor? Instructor { get; set; }
        public string? InstructorId { get; set; } = string.Empty;
        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }
    }
}
