using InternshipApp.Contracts;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.Navigation;

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
    public StudentManager Students { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string JobId { get; set; }

    [Parameter]
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

            this.StateHasChanged();

            var job = await Jobs.FindByIdAsync(int.Parse(JobId));
            var student = await Students.FindByIdAsync(StudentId);
            if (job == null || student == null)
            {
                this.States = null;
                return;
            }

            var item = new ApplicationDetailsViewStates()
            {
                StudentId = student.Id,
                JobId = job.Id,
                StudentName = student.FullName,
                JobName = job.Title
            };

            if (item == null)
            {
                this.States = null;
                return;
            }

            this.States = item;
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