using Wave5.UI;

namespace Wave5.UI.Blazor;

public static class DetailsListContextExtensions
{
    #region [ Public Methods - SetLoadingStates ]
    public static void SetProcessingStates<T>(this DetailsListContext<T> context, bool isProcessing) {
        if (isProcessing) {
            context.ClearSelectedItems();
        }
    }
    #endregion
}
