using Microsoft.AspNetCore.Components;
using RCode;
using Wave5.UI.Forms;
using Wave5.UI.Navigation;
using Wave5.UI;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Wave5.UI.Blazor;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class ManageInternView
{
    #region [ CTor ]
    public ManageInternView()
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
    public string StudentId { get; set; }
    #endregion

    #region [ Properties - Contexts ]
    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsCardContainerContext DetailsContainerContext { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected ApplicationDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.CommandBarContext = new CommandBarContext();
        this.DetailsContainerContext = new DetailsCardContainerContext();

        this.InitializeCommandBars();

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentJobId = this.JobId;
        var parameterJobId = parameters.GetValueOrDefault<string>(nameof(this.JobId)); 
        var currentStudentId = this.StudentId;
        var parameterStudentId = parameters.GetValueOrDefault<string>(nameof(this.StudentId));

        await base.SetParametersAsync(parameters);

        if (currentJobId != parameterJobId || currentStudentId != parameterStudentId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Event Handlers - CommandBar ]
    protected async void OnRefreshButtonClicked(EventArgs args)
    {
        await LoadDataAsync();
    }
    #endregion

    #region [ Private Methods - CommandBars ]
    private void InitializeCommandBars()
    {
        this.CommandBarContext.Items.AddRefreshButton(this.OnRefreshButtonClicked);
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.JobId, nameof(this.JobId));
        Guard.ParamIsNullOrEmpty(this.StudentId, nameof(this.StudentId));

        try
        {
            this.DetailsContainerContext.SetProcessingStates(true, false);
            this.CommandBarContext.SetProcessingStates(true);

            var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId))
                .Include(x => x.StudentJobs.Where(x => x.StudentId == StudentId))
                .FirstOrDefaultAsync();

            if (job == null)
            {

            }

            var item = job.StudentJobs.FirstOrDefault(x => x.StudentId == StudentId);

            if (job == null || item == null)
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