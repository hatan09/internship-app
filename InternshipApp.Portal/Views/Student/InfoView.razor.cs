using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RCode;
using Syncfusion.Blazor.RichTextEditor;
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
    public ISkillRepository Skills { get; set; }

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
    public FormRequest<FormAction, Student> StudentFormRequest { get; private set; }
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

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<Student> result)
    {
        switch (result.State)
        {
            case FormResultState.Added:
            case FormResultState.Updated:
            case FormResultState.Deleted:
                var tasks = new List<Task>
                {
                    this.LoadDataAsync()
                };

                await Task.WhenAll(tasks);
                break;
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.StudentId, nameof(this.StudentId));

        try
        {
            var student = await this.Students.FindAll(x => x.Id == StudentId).AsNoTracking().Include(x => x.StudentSkills).Include(x => x.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED)).FirstOrDefaultAsync();

            if (student is null)
            {
                this.States = null;
                return;
            }

            this.States = student.ToDetailsViewStates();

            var skills = await Skills.FindAll().AsNoTracking().ToListAsync();
            States.AllSkills = skills;
            States.StudentSkills = student.StudentSkills.ToList();

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

    public void OnEdit()
    {
        try
        {
            StudentFormRequest = FormRequestFactory.EditRequest(States.ToEntity());
            StateHasChanged();
        }
        catch (Exception ex)
        {

        }
    }

    public async Task OnReject()
    {
        var student = await Students.FindAll(x => x.Id == States.Id).AsTracking().FirstOrDefaultAsync();
        var group = await Groups.FindAll(x => x.Students.Contains(student)).Include(x => x.Students).AsTracking().FirstOrDefaultAsync();
        if (student != null)
        {
            if(group != null)
            {
                group.Students.Remove(student);
                Groups.Update(group);
                await Groups.SaveChangesAsync();
            }

            student.Stat = Stat.REJECTED;

            var result = await Students.UpdateAsync(student);
            if (result.Succeeded)
            {
                States.Status = Stat.REJECTED.ToString();
                NavigationManager.NavigateTo("/students", true);
            }
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

    public async Task OnUndo()
    {
        var student = await Students.FindByIdAsync(States.Id);
        student.Stat = Stat.WAITING;
        var result = await Students.UpdateAsync(student);
        if (result.Succeeded)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Student with ID {student.StudentId} has been recovered!");
            await LoadDataAsync();
        }
        else
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Update was failed for student with ID {student.StudentId}.");
        }
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
