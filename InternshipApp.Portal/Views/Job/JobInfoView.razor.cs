using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;
using Syncfusion.Blazor.Schedule.Internal;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class JobInfoView
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
    public StudentManager Students { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }
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
        States = new();
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

    #region [ Private Methods - Data ]
    private async void OnApply()
    {
        var student = await GetStudentAsync();
        if(student.StudentJobs.FirstOrDefault(x => x.JobId == int.Parse(JobId)) != null)
        {
            return;
        }
        var studentJob = new StudentJob()
        {
            StudentId = student.Id.ToString(),
            JobId = int.Parse(JobId)
        };

        await OnUpdateApplicationAsync(studentJob);
    }

    private async Task<Student> GetStudentAsync()
    {
        var student = await Students.FindAll().FirstOrDefaultAsync();

        return student;
    }

    private async Task OnUpdateApplicationAsync(StudentJob studentJob)
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).Include(x => x.StudentJobs).FirstOrDefaultAsync();
        job?.StudentJobs.Add(studentJob);
        Jobs.Update(job);
        await Jobs.SaveChangesAsync();
    }

    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.JobId, nameof(this.JobId));

        try
        {
            var student = await GetStudentAsync();

            var item = await this.Jobs.FindAll(x => x.Id == int.Parse(JobId))
                .Include(x => x.JobSkills)
                .Include(x => x.StudentJobs.Where(y => y.StudentId == student.Id))
                .FirstOrDefaultAsync();

            if (item is null)
            {
                this.States = null;
                return;
            }


            this.States = item.ToDetailsViewStates();
            States.HasApplied = item.StudentJobs.Any();
            States.JobSkills = item.JobSkills.ToList();
            var skillIds = item.JobSkills.Select(x => x.SkillId).ToList();

            skillIds.ForEach(async x =>
            {
                if(x > 0)
                {
                    States.Skills.Add(await Skills.FindByIdAsync(x));
                }
            });
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
