using Wave5.UI;

namespace Wave5.UI.Blazor;

public static class DetailsCardContainerContextExtensions
{
    #region [ Public Methods - SetLoadingStates ]
    public static void SetProcessingStates(this DetailsCardContainerContext context, bool isLoading, bool hasData) {
        context.IsLoading = isLoading;
        context.HasData = hasData;
    }
    #endregion
}