using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RCode;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class JobInfoView
{
    #region [ Fields ]
    public bool HasApplied { get; set; }
    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string JobId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public IMatchingService MatchingService { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }
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

        if(!States.IsAllowedApply)
        {
            await JSRuntime.InvokeVoidAsync("alert", "Can't Apply");
            return;
        }
        var studentJob = new StudentJob()
        {
            StudentId = student.Id.ToString(),
            JobId = int.Parse(JobId)
        };

        await OnUpdateApplicationAsync(studentJob);
        if(student.Stat == Stat.WAITING)
        {
            student.Stat = Stat.APPLIED;
            await Students.UpdateAsync(student);
        }
        await LoadDataAsync();

    }

    private async Task<Student> GetStudentAsync()
    {
        var user = await LocalStorage.GetItemAsync<User>("login-user-info");
        string studentId = string.Empty;
        if(user != null)
        {
            studentId = user.Id;
        }
        var student = await Students.FindAll(x => x.Id == studentId)
            .AsTracking()
            .FirstOrDefaultAsync();

        return student;
    }

    private async Task OnUpdateApplicationAsync(StudentJob studentJob)
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).AsTracking().Include(x => x.StudentJobs).FirstOrDefaultAsync();
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

            var job = await this.Jobs.FindAll(x => x.Id == int.Parse(JobId))
                .Include(x => x.Company)
                .Include(x => x.JobSkills)
                .Include(x => x.StudentJobs)
                .FirstOrDefaultAsync();

            if (job is null)
            {
                this.States = null;
                return;
            }

            var existingInterns = job.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED).Count();
            var remaining = job.Slots - existingInterns;

            this.States = job.ToDetailsViewStates();
            HasApplied = job.StudentJobs.Where(x => x.StudentId == student.Id).Any() && student.Stat != Stat.FINISHED && student.Stat != Stat.HIRED;
            States.IsAllowedApply = remaining > 0
                                    && !HasApplied
                                    && student.InternGroupId != null
                                    && student.Stat != Stat.FINISHED 
                                    && student.Stat != Stat.HIRED;
            States.JobSkills = job.JobSkills.ToList();
            var skillIds = job.JobSkills.Select(x => x.SkillId).ToList();

            var company = job.Company;
            if(company != null)
            {
                States.CompanyName = company.Title;
                States.Address = company.Address;
            }

            States.Remaining = remaining;

            var matching = await MatchingService.GetMatchingPointById(student.Id, job.Id);
            States.Matching = matching;

            var skills = await Skills.FindAll(x => skillIds.Contains(x.Id)).AsNoTracking().ToListAsync();
            States.Skills.AddRange(skills);
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
