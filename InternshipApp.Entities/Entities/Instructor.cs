namespace InternshipApp.Core.Entities
{
    public class Instructor : User
    {
        public int? DepartmentId { get; set; }
        public bool IsDepartmentManager { get; set; } = false;
        public virtual Department? Department { get; set; }
        public virtual InternGroup? InternGroup { get; set; }
        //public bool IsGroupManager { get; set; } = false;
    }
}
