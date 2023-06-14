using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;
using Wave5.UI;
using Wave5.UI.Blazor;

namespace InternshipApp.Portal.Views;

public partial class HireDetailsView
{
    #region [ Properties - Inject ]
    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }
    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string StudentId { get; set; }
    #endregion

    #region [ Properties - Contexts ]
    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsCardContainerContext DetailsContainerContext { get; private set; }

    protected bool IsHired { get; set; }
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
        var currentStudentId = this.StudentId;
        var parameterStudentId = parameters.GetValueOrDefault<string>(nameof(this.StudentId));

        await base.SetParametersAsync(parameters);

        if (currentStudentId != parameterStudentId)
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
        Guard.ParamIsNullOrEmpty(this.StudentId, nameof(this.StudentId));

        try
        {
            this.DetailsContainerContext.SetProcessingStates(true, false);
            this.CommandBarContext.SetProcessingStates(true);

            this.StateHasChanged();

            var student = await Students
                .FindAll(x => x.Id == StudentId)
                .Include(x => x.StudentJobs.FirstOrDefault(x => x.Status == ApplyStatus.HIRED))
                .FirstOrDefaultAsync();
            if (student == null)
            {
                this.States = null;
                return;
            }

            IsHired = student.StudentJobs.Count == 1;
            if (IsHired)
            {
                var job = await Jobs.FindByIdAsync(student.StudentJobs.First().JobId?? 0);
                if(job == null)
                {
                    this.States = null; 
                    return;
                }

                States = new ApplicationDetailsViewStates()
                {
                    StudentId = student.Id,
                    JobId = job.Id,
                    StudentName = student.FullName,
                    JobName = job.Title
                };
            }
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
