namespace InternshipApp.Core.Entities;

public class Evaluation : BaseEntity<int>
{
    public string? StudentId { get; set; }
    public int? JobId { get; set; }
    public string Title { get; set; } = "Evaluation";
    public int Score { get; set; } = 0;
    public Performance Performance { get; set; } = Performance.AVERAGE;
    public string? ProjectName { get; set; } = string.Empty;
    public string? Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow.Date;

    public string? ReportUrl { get; set; }
}

public enum Performance { POOR, AVERAGE, GOOD, EXCELLENT }
