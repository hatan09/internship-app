using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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

    [Inject]
    public StudentManager Students { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Job> ApplicationFormRequest { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsCardContainerContext DetailsContainerContext { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected JobDetailsViewStates States { get; private set; }

    protected List<ApplicationListRowViewStates> Data { get; private set; }
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
        this.CommandBarContext.Items.AddEditButton(OnEditButtonClicked, true);
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

    protected void OnEditButtonClicked(EventArgs args)
    {
        ApplicationFormRequest = FormRequestFactory.EditRequest(States.ToEntity());
        StateHasChanged();
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

            var item = await this.Jobs
                .FindAll(x => x.Id == int.Parse(JobId)).AsNoTracking()
                .Include(x => x.StudentJobs
                    .Where(x => x.Status == ApplyStatus.HIRED))
                .FirstOrDefaultAsync();

            if (item is null)
            {
                this.States = null;
                return;
            }

            var interns = item.StudentJobs.ToList();

            var allStudents = await Students.FindAll(x => interns.Select(x => x.StudentId).Contains(x.Id)).ToListAsync();
            Data = interns.ToListRowList();
            Data.ForEach(x => {
                x.JobName = item.Title;
                x.StudentName = allStudents.FirstOrDefault(y => y.Id == x.StudentId)?.FullName;
            });

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
