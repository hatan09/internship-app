namespace InternshipApp.Portal.Views;

public partial class StudentInfoFormPanel
{
    private bool IsOpen { get; set; }


    #region [ Event Handlers ]
    private async void OnPanelDismissed()
    {
        await this.OnDismissedAsync();
    }

    private async Task OnDismissedAsync()
    {
        this.SetIsOpenState(false);
        await Task.Delay(0);
    }
    #endregion

    #region [ Private Methods - Helpers ]
    private void SetIsOpenState(bool isOpen)
    {
        this.IsOpen = isOpen;
        this.StateHasChanged();
    }
    #endregion
}
