namespace Wave5.ProjectPortal.Views;

using Wave5.UI;

public static class AppStyles
{
    #region [ Public Methods - AppPage ]
    public static string AppPageStyle = "background:#faf9f8;padding:0;margin:0;";
    #endregion

    #region [ Public Methods - AppPageContainer ]
    public static string AppPageContainerStyle = "margin-top:10px; margin:28px;";

    public static string ProjectDetailContainerStyle = "margin-top:20px;";

    public static StackTokens AppPageContainerTokens = new StackTokens() { ChildrenGap = new[] { 10.0 } };
    #endregion
}