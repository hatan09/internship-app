using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using InternshipApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RCode;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views; 
public partial class ApplicationInfoView 
{
    #region [ Properties - Parameter ]
    [Parameter]
    public string StudentId { get; set; }

    [Parameter]
    public string JobId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public IMatchingService MatchingService { get; set; }

    [Inject]
    public IEmailService EmailService { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, StudentJob> ApplicationFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected ApplicationDetailsViewStates States { get; private set; }

    protected List<Skill> AllSkills { get; private set; }

    public PopupContext SendEmailPopupContext { get; set; }
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
            States.StudentEmail = student.Email;
            States.Year = student.Year;
            States.Credits = student.Credit;
            States.Gpa = student.GPA;
            States.Bio = student.Bio;
            States.Matching = await MatchingService.GetMatchingPointById(StudentId, int.Parse(JobId));
            States.GitHubUrl = student.GitProfileUrl;
            States.CvUrl = student.CVUrl;
            States.ImgUrl = student.ImgUrl;
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

    public async void OnOpenGithubProfile()
    {
        if (string.IsNullOrEmpty(States.GitHubUrl)) return;

        if (!States.GitHubUrl.Contains("http://") && !States.GitHubUrl.Contains("https://"))
            States.GitHubUrl = "https://" + States.GitHubUrl;
        await JSRuntime.InvokeVoidAsync("open", States.GitHubUrl, "_blank");
    }

    public async void OnOpenCvUrl()
    {
        if (string.IsNullOrEmpty(States.CvUrl)) return;

        if (!States.CvUrl.Contains("http://") && !States.CvUrl.Contains("https://"))
            States.CvUrl = "https://" + States.CvUrl;
        await JSRuntime.InvokeVoidAsync("open", States.CvUrl, "_blank");
    }
    #endregion

    #region [ Protected Methods - CommandBar ]
    public async void OnAccept()
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId))
                            .Include(x => x.StudentJobs.Where(x => x.StudentId == StudentId))
                            .FirstOrDefaultAsync();

        if (job == null)    // can't find job
        {
            return;
        }

        if(job.StudentJobs.Count < 1)   // student hasn't applied for this job
        {
            return;
        }

        if(job.Slots <= job.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED).Count())  // no vacancy left
        {
            return;
        }

        if(job.StudentJobs.First().Status == ApplyStatus.WAITING)
        {
            var student = await Students.FindAll(x => x.Id == StudentId)
                .AsTracking()
                .FirstOrDefaultAsync();
            if(student.Stat == Stat.WAITING)
            {
                student.Stat = Stat.APPLIED;
                await Students.UpdateAsync(student);
            }
            job.StudentJobs.First().Status = ApplyStatus.ACCEPTED;

        }
        else if(job.StudentJobs.First().Status == ApplyStatus.ACCEPTED)
        {
            job.StudentJobs.First().Status = ApplyStatus.HIRED;
            var student = await Students.FindAll(x => x.Id == StudentId)
                .Include(x => x.StudentJobs.Where(x => x.JobId != job.Id && x.Status != ApplyStatus.REJECTED))
                .AsTracking()
                .FirstOrDefaultAsync();
            student.Stat = Stat.HIRED;
            foreach(var x in student.StudentJobs)
            {
                x.Status = ApplyStatus.MISSED;
            }
            await Students.UpdateAsync(student);
        }
        Jobs.Update(job);
        await Jobs.SaveChangesAsync();
        await LoadDataAsync();
    }

    public void OnSendEmail()
    {
        SendEmailPopupContext = new()
        {
            IsOpen = true,
            StudentName = States.StudentName,
            OnSendEmailCallback = SendEmailAsync
        };
    }

    public async void SendEmailAsync(string subject, string content)
    {
        await EmailService.Send(new()
        {
            To = States.StudentEmail,
            Subject = subject,
            Body = content
        });
    }

    public async void OnReject()
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId))
                            .Include(x => x.StudentJobs.Where(x => x.StudentId == StudentId))
                            .FirstOrDefaultAsync();

        if (job == null)
        {
            return;
        }

        if (job.StudentJobs.Count < 1)
        {
            return;
        }

        job.StudentJobs.First().Status = ApplyStatus.REJECTED;
        Jobs.Update(job);
        await Jobs.SaveChangesAsync();
        await LoadDataAsync();
    }
    #endregion
}
