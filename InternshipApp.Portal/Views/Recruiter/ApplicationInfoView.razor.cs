using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views; 
public partial class ApplicationInfoView 
{
    #region [ Fields ]

    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string StudentId { get; set; }

    [Parameter]
    public string JobId { get; set; }
    #endregion

    #region [ Properties ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public IMatchingService MatchingService { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, StudentJob> ApplicationFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected ApplicationDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentStudentId = this.StudentId;
        var parameterStudentId = parameters.GetValueOrDefault<string>(nameof(this.StudentId));

        var currentJobId = this.JobId;
        var parameterJobId = parameters.GetValueOrDefault<string>(nameof(this.JobId));

        await base.SetParametersAsync(parameters);

        if (currentStudentId != parameterStudentId || currentJobId != parameterJobId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.StudentId, nameof(this.StudentId));

        try
        {
            var job = await Jobs.FindByIdAsync(int.Parse(this.JobId));

            var student = await this.Students
                .FindAll(x => x.Id == StudentId)
                .Include(x => x.StudentSkills)
                .Include(x => x.StudentJobs.Where(x => x.JobId == int.Parse(JobId)))
                .FirstOrDefaultAsync();
            var item = student.StudentJobs.FirstOrDefault();
            if (job == null || student == null || item == null)
            {
                this.States = null;
                return;
            }

            this.States = item.ToDetailsViewStates();
            States.StudentName = student.FullName;
            States.Year = student.Year;
            States.Credits = student.Credit;
            States.Gpa = student.GPA;
            States.Bio = student.Bio;
            States.Matching = await MatchingService.GetMatchingPointById(StudentId, int.Parse(JobId));
            States.JobName = job.Title;
            
        }
        catch (Exception ex)
        {

        }
        finally
        {
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
