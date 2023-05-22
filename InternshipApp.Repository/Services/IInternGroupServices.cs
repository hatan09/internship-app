namespace InternshipApp.Contracts;

public interface IInternGroupServices
{
    public Task AutoCreateAndAssign(int maxAmount = 0);

    public Task AutoAssign(bool isDividedEqually = true);
}
