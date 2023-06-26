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
    #region [ Properties ]
    public bool HasApplied { get; set; }

    public bool IsStudentViewing { get; set; }
    public bool IsTeacherViewing { get; set; }
    public bool IsRecruiterViewing { get; set; }
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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            var role = await LocalStorage.GetItemAsStringAsync("role");
            switch (role)
            {
                case "STUDENT":
                    IsStudentViewing = true;
                    break;
                case "INSTRUCTOR":
                    IsTeacherViewing = true;
                    break;
                case "RECRUITER":
                    IsRecruiterViewing = true;
                    break;
                default:
                    break;
            }
            await LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Private Methods - Data ]
    private async void OnCopy()
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", NavigationManager.Uri);
    }

    private async void OnAccept()
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).AsTracking().FirstOrDefaultAsync();
        if (job == null || job.IsAccepted)
        {
            return;
        }

        job.IsAccepted = true;
        Jobs.Update(job);
        await Jobs.SaveChangesAsync();
        await JSRuntime.InvokeVoidAsync("alert", "Job has been accepted!");
    }

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

        var isApplied = await OnUpdateApplicationAsync(studentJob);
        if(isApplied)
        {
            if (student.Stat == Stat.WAITING)
            {
                student.Stat = Stat.APPLIED;
                await Students.UpdateAsync(student);
            }
            await JSRuntime.InvokeVoidAsync("alert", "Applied Successfully!");
            await LoadDataAsync();
            return;
        }
        else
        {
            await JSRuntime.InvokeVoidAsync("alert", "An error occured while applying fot job. Please try again later!");
            return;
        }

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

    private async Task<bool> OnUpdateApplicationAsync(StudentJob studentJob)
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).AsTracking().Include(x => x.StudentJobs).FirstOrDefaultAsync();
        if(job == null || job.Slots - job.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED).Count() <= 0)
        {
            return false;
        }
        job?.StudentJobs.Add(studentJob);
        Jobs.Update(job);
        await Jobs.SaveChangesAsync();
        return true;
    }

    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.JobId, nameof(this.JobId));

        try
        {
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

            this.States = job.ToDetailsViewStates();

            var existingInterns = job.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED).Count();
            var remaining = job.Slots - existingInterns;
            States.Remaining = remaining;

            if (IsStudentViewing)
            {
                var student = await GetStudentAsync();
                HasApplied = job.StudentJobs.Where(x => x.StudentId == student.Id).Any() && student.Stat != Stat.FINISHED && student.Stat != Stat.HIRED;
                States.IsAllowedApply = States.Remaining > 0
                                        && !HasApplied
                                        && student.InternGroupId != null
                                        && student.Stat != Stat.FINISHED
                                        && student.Stat != Stat.HIRED;
                States.Matching = await MatchingService.GetMatchingPointById(student.Id, job.Id);
            }

            States.JobSkills = job.JobSkills.ToList();
            var skillIds = job.JobSkills.Select(x => x.SkillId).ToList();

            if(job.Company != null)
            {
                States.CompanyName = job.Company.Title;
                States.Address = job.Company.Address;
            }


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
