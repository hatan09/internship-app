namespace Wave5.UI.Blazor;

public static class DataListSearchContextExtensions
{
    #region [ Public Methods - SetLoadingStates ]
    public static void SetProcessingStates(this DataListSearchContext context, bool isProcessing) {
        context.SetDisableStates(isProcessing);
    }
    #endregion
}