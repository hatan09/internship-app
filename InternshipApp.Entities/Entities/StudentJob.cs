namespace InternshipApp.Core.Entities
{
    public class StudentJob
    {
        public ApplyStatus Status { get; set; } = ApplyStatus.WAITING;
        public bool IsAccepted { get; set; }
        public Student? Student { get; set; }
        public string? StudentId { get; set; } = string.Empty;
        public Job? Job { get; set; }
        public int? JobId { get; set; }

        public int? Score { get; set; } = 0;
    }

    public enum ApplyStatus { WAITING, ACCEPTED, HIRED, REJECTED, FINISHED }
}
