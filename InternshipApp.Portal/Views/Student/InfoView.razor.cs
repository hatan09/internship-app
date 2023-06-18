using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RCode;
using Wave5.UI;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class InfoView
{
    #region [ Fields ]
    public bool IsAdminViewing { get; set; }

    public bool IsTeacherViewing { get; set; }

    public bool IsHired { get; set; }

    public bool IsModalOpen { get; set; }

    public List<string> StudentIds { get; set; }

    public PopupContext PopupContext { get; set; }
    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string StudentId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IInternGroupRepository Groups { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public IEvaluationRepository Evaluations { get; set; }

    [Inject]
    public ICompanyRepository Companies { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; private set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Student> ApplicationFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected StudentDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentApplicationId = this.StudentId;
        var parameterApplicationId = parameters.GetValueOrDefault<string>(nameof(this.StudentId));

        await base.SetParametersAsync(parameters);

        if (currentApplicationId != parameterApplicationId)
        {
            await this.LoadDataAsync();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var role = await LocalStorage.GetItemAsStringAsync("role");
        if (role.Contains("INSTRUCTOR"))
        {
            IsTeacherViewing = true;
        }
        else if (role.Contains("ADMIN"))
        {
            IsAdminViewing = true;
        }
        if (firstRender) {
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.StudentId, nameof(this.StudentId));

        try
        {
            var student = await this.Students.FindAll(x => x.Id == StudentId).AsNoTracking().Include(x => x.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED)).FirstOrDefaultAsync();

            if (student is null)
            {
                this.States = null;
                return;
            }

            this.States = student.ToDetailsViewStates();

            if (student.Stat == Stat.HIRED)
            {
                IsHired = true;
                var studentJob = student.StudentJobs.FirstOrDefault();
                if (studentJob != null)
                {
                    var job = await Jobs.FindAll(x => x.Id == (studentJob.JobId ?? 0)).AsNoTracking().Include(x => x.Company).FirstOrDefaultAsync();
                    if (job != null)
                    {
                        States.JobName = job.Title;
                        States.CompanyName = job.Company?.Title;
                    }
                }
            }

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

    public void OnChat()
    {
        NavigationManager.NavigateTo($"/chat/{States.Id}");
    }

    public async void OnSendEmail()
    {

    }

    public async void OnReject()
    {
        var student = await Students.FindAll(x => x.Id == States.Id).AsTracking().FirstOrDefaultAsync();
        if(student != null)
        {
            student.Stat = Stat.REJECTED;
            States.Status = Stat.REJECTED.ToString();
            StateHasChanged();
        }
    }

    public void OnShowScore()
    {
        PopupContext = new() {
            IsOpen = true,
            StudentId = StudentId
        };
    }

    public async Task OnAddToGroup()
    {
        var user = await LocalStorage.GetItemAsync<User>("login-user-info");
        if(user == null)
        {
            NavigationManager.NavigateTo("/", true);
            return;
        }

        var group = await Groups.FindAll(x => x.InstructorId == user.Id).Include(x => x.Students).AsTracking().FirstOrDefaultAsync();
        var student = await Students.FindByIdAsync(States.Id);
        if (group != null && student != null && student.Stat == Stat.WAITING)
        {
            group.Students.Add(student);
            Groups.Update(group);
            await Groups.SaveChangesAsync();
        }
    }

    public async Task OnFinish()
    {
        var student = await Students.FindAll(x => x.Id == States.Id).Include(x => x.StudentJobs).AsTracking().FirstOrDefaultAsync();
        if(student == null)
        {
            await JSRuntime.InvokeVoidAsync("alert", "Can't mark as finished");
            return;
        }

        var evaluations = await Evaluations.FindByStudentAsync(student.Id);
        var score = evaluations.Average(x => x.Score);

        student.Stat = Stat.FINISHED;
        student.Score = (int) score;
        student.StudentJobs.Clear();

        var result = await Students.UpdateAsync(student);
        if(!result.Succeeded)
        {
            await JSRuntime.InvokeVoidAsync("alert", "Can't mark as finished");
            return;
        }
        await LoadDataAsync();
    }

    public void OnToggleModal()
    {
        StudentIds = new()
        {
            States.Id
        };
        IsModalOpen = !IsModalOpen;
    }

    public async void OnUpdatedCallBack()
    {
        IsModalOpen = false;
        await LoadDataAsync();
    }

    public async void OnOpenGithubProfile()
    {
        if (string.IsNullOrEmpty(States.GitUrl)) return;

        if (!States.GitUrl.Contains("http://") && !States.GitUrl.Contains("https://"))
            States.GitUrl = "https://" + States.GitUrl;
        await JSRuntime.InvokeVoidAsync("open", States.GitUrl, "_blank");
    }

    public async void OnOpenCVUrl()
    {
        if (string.IsNullOrEmpty(States.CVUrl)) return;

        if (!States.CVUrl.Contains("http://") && !States.CVUrl.Contains("https://"))
            States.CVUrl = "https://" + States.CVUrl;
        await JSRuntime.InvokeVoidAsync("open", States.CVUrl, "_blank");
    }
    #endregion
}
