using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class InternGroupFormViewPanel
{
    private bool IsOpen { get; set; }

    #region [ Properties - Parameters ]
    [Parameter]
    public FormRequest<FormAction, InternGroup> FormRequest { get; set; }

    [Parameter]
    public EventCallback<FormResult<InternGroup>> FormResultCallback { get; set; }
    #endregion

    #region [ Event Handlers - Override ]
    public override async Task SetParametersAsync(ParameterView parameters)
    {

        var newActionRequest = default(FormRequest<FormAction, InternGroup>);
        parameters.TryGetValue(nameof(this.FormRequest), out newActionRequest);

        if (newActionRequest != this.FormRequest)
        {
            this.SetIsOpenState(newActionRequest != null);
        }

        await base.SetParametersAsync(parameters);
    }
    #endregion

    #region [ Event Handlers ]
    private async void OnPanelDismissed()
    {
        await this.OnDismissedAsync();
    }

    private async void OnFormResultReceived(FormResult<InternGroup> result)
    {
        await this.OnDismissedAsync(result);
    }

    private async Task OnDismissedAsync(FormResult<InternGroup> result = null)
    {
        this.SetIsOpenState(false);

        result ??= FormResultFactory.CanceledResult(this.FormRequest.Data);
        await this.FormResultCallback.InvokeAsync(result);
    }

    private async Task OnDismissedAsync()
    {
        this.SetIsOpenState(false);
        await Task.Delay(0);
        StateHasChanged();
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
