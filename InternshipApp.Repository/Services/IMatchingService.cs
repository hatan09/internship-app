namespace InternshipApp.Repository;

public interface IMatchingService
{
    #region [ Public Methods - Ranking ]
    public Task<int> GetMatchingPoint(string studentId, int jobId);
    #endregion
}
