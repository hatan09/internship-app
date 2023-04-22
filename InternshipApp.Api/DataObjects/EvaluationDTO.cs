using InternshipApp.Core.Entities;

namespace InternshipApp.Api.DataObjects;

public class EvaluationDTO : BaseDTO<int>
{
    public string? StudentId { get; set; } = string.Empty;
    public int? JobId { get; set; } = null;
    public string Title { get; set; } = "Evaluation";
    public int Score { get; set; } = 0;
    public PerformanceRank Performance { get; set; } = PerformanceRank.AVERAGE;
    public string? ProjectName { get; set; } = string.Empty;
    public string? Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow.Date;

    public string? ReportUrl { get; set; } = "No report";
}
