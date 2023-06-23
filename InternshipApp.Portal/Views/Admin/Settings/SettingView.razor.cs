using InternshipApp.Contracts;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class SettingView
{
	#region [ Properties - Inject ]
	[Inject]
	public IInternSettingsRepository Settings { get; set; }
    #endregion

    #region [ Properties - States ]
    public InternSettingsDetailsViewStates States { get; set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadDataAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        var settings = await Settings.GetCurrentSemester();
        if(settings != null)
        {
            States = settings.ToDetailsViewStates();
            StateHasChanged();
        }
    }
    #endregion
}
