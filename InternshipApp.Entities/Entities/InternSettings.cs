namespace InternshipApp.Core.Entities;

public class InternSettings : BaseEntity<int>
{
    public DateTime StartTime { get; set; }
    public DateTime CloseRegistrationTime { get; set; } //get admin => "today is close registration day. please create groups"
    public DateTime JobDeadline { get; set; } //1 week: get ins, rec, WAITING+APPLIED => "1 week left", today: get ins, rec, WAITING+APPLIED => "today is the deadline", 1d past: get WAITING+APPLIED => REJECTED + "You are not qualified for this internship"
    public DateTime SummaryTime { get; set; }   //1 week: get rec, HIRED => "please submit evaluation", today: get rec, HIRED => "instructors from IU is going to summarize students' progress after today. please submit evalutaion.", 1d past: get ins with group => "from today until endtime, please summarize student evaluation and scores"
    public DateTime EndTime { get; set; }   //1 week: get rec, HIRED, ins => "1 week left before this internship program ends. please summarize students' scores soon", today: "today is the final day of this internship. please summarize students' scores soon", 1d past: get admin => "please end this internship"
    public int MaximumStudents { get; set; }
    public string Title { get; set; } = string.Empty;


}
