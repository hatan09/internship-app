namespace InternshipApp.Core.Entities;

public class InternSettings : BaseEntity<int>
{
    public DateTime StartTime { get; set; }
    public DateTime CloseRegistrationTime { get; set; }
    public DateTime JobDealine { get; set; }
    public DateTime SummaryTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaximumStudents { get; set; }
    public string Title { get; set; } = string.Empty;


}
