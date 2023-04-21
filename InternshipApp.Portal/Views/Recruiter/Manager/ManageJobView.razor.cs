using Microsoft.AspNetCore.Components;
using RCode;
using Wave5.UI.Forms;
using Wave5.UI.Navigation;
using Wave5.UI;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Wave5.UI.Blazor;

namespace InternshipApp.Portal.Views;

public partial class ManageJobView
{
    #region [ CTor ]
    public ManageJobView()
    {

    }
    #endregion

    #region [ Properties - Inject ]

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public INavigationMenuProvider MainMenuProvider { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string JobId { get; set; }
    #endregion

    #region [ Properties - Contexts ]
    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<JobListRowViewStates> ListContext { get; private set; }

    protected DetailsCardContainerContext DetailsContainerContext { get; private set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Job> JobFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected JobDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.SearchContext = new DataListSearchContext();
        this.CommandBarContext = new CommandBarContext();
        this.DetailsContainerContext = new DetailsCardContainerContext();
        this.ListContext = new DetailsListContext<JobListRowViewStates>();
        this.ListContext.SelectionMode = SelectionMode.Single;
        this.ListContext.OnItemInvoked += this.OnRowClicked;

        this.InitializeCommandBars();

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentJobId = this.JobId;
        var parameterJobId = parameters.GetValueOrDefault<string>(nameof(this.JobId));

        await base.SetParametersAsync(parameters);

        if (currentJobId != parameterJobId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Event Handlers - CommandBar ]
    protected void OnEditButtonClicked(EventArgs args)
    {
        this.JobFormRequest = FormRequestFactory.EditRequest(this.States.ToEntity());
        this.StateHasChanged();
    }

    protected async void OnRefreshButtonClicked(EventArgs args)
    {
        await LoadDataAsync();
    }

    protected void OnDetailsButtonClicked(EventArgs args)
    {
        this.JobFormRequest = FormRequestFactory.DetailsRequest(this.States.ToEntity());
        this.StateHasChanged();
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

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(JobListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/manage-job-info/{rowItem.Id}");
    }
    #endregion

    #region [ Private Methods - CommandBars ]
    private void InitializeCommandBars()
    {
        this.CommandBarContext.Items.AddRefreshButton(this.OnRefreshButtonClicked);
        this.CommandBarContext.Items.AddEditButton(this.OnEditButtonClicked, true);
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

            var item = await Jobs.FindByIdAsync(int.Parse(JobId));

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
    #endregion
}