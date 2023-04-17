using InternshipApp.Contracts;
using Microsoft.AspNetCore.Components;
using RCode;

namespace InternshipApp.Portal.Views;

public partial class CompanyInfo : ComponentBase
{
	#region [ Properties ]
	[Inject]
	public ICompanyRepository Companies { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
	public string CompanyId { get; set; }

    public CompanyDetailsViewStates States { get; set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentLinkedServiceId = this.CompanyId;
        var parameterLinkedServiceId = parameters.GetValueOrDefault<string>(nameof(this.CompanyId));

        await base.SetParametersAsync(parameters);

        if (currentLinkedServiceId != parameterLinkedServiceId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.CompanyId, nameof(this.CompanyId));

        try
        {
            var item = await this.Companies.FindByIdAsync(int.Parse(this.CompanyId));

            if (item is null)
            {
                this.States = null;
                return;
            }

            this.States = item.ToDetailsViewStates();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.StateHasChanged();
        }
    }
    #endregion
}
