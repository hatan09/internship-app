using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using RCode;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class ManageJobView
{
    #region [ Fields ]

    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string JobId { get; set; }
    #endregion

    #region [ Properties ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Job> ApplicationFormRequest { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsCardContainerContext DetailsContainerContext { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected JobDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.CommandBarContext = new CommandBarContext();
        this.DetailsContainerContext = new DetailsCardContainerContext();
        States = new();
        this.InitializeCommandBars();

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentApplicationId = this.JobId;
        var parameterApplicationId = parameters.GetValueOrDefault<string>(nameof(this.JobId));

        await base.SetParametersAsync(parameters);

        if (currentApplicationId != parameterApplicationId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Private Methods - CommandBars ]
    private void InitializeCommandBars()
    {
        this.CommandBarContext.Items.AddEditButton(null, true);
    }
    #endregion

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<Job> result)
    {
        switch (result.State)
        {
            case FormResultState.Added:
            case FormResultState.Updated:
            case FormResultState.Deleted:
                await this.LoadDataAsync();
                break;
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.JobId, nameof(this.JobId));

        try
        {
            this.DetailsContainerContext.SetProcessingStates(true, false);
            this.CommandBarContext.SetProcessingStates(true);

            var item = await this.Jobs.FindByIdAsync(int.Parse(this.JobId));

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
            this.DetailsContainerContext.SetProcessingStates(false, this.States != null);
            this.CommandBarContext.SetProcessingStates(false, this.States != null);
            this.StateHasChanged();
        }
    }

    public string GetOrderFromInt(int order)
    {
        switch (order)
        {
            case 1:
                {
                    return "First";
                }
            case 2:
                {
                    return "Second";
                }
            case 3:
                {
                    return "Third";
                }
            case 4:
                {
                    return "Fourth";
                }
            case 5:
                {
                    return "Fifth";
                }
            case 6:
                {
                    return "Sixth";
                }
            default:
                {
                    return "";
                }
        }
    }

    public async void OnChat()
    {

    }
    #endregion
}
