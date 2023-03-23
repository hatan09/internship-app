using Wave5.UI;

namespace Wave5.UI.Blazor;

public static class DetailsListContainerContextExtensions
{
    #region [ Public Methods - SetLoadingStates ]
    public static void SetProcessingStates(this DetailsListContainerContext context, bool isProcessing, bool hasData) {
        context.IsLoading = isProcessing;
        context.HasData = hasData;
    }
    #endregion
}