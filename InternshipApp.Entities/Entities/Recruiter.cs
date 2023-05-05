namespace InternshipApp.Core.Entities
{
    public class Recruiter : User
    {
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }
    }
}
