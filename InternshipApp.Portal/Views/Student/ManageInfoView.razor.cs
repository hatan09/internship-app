using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class ManageInfoView
{
    #region [ Fields ]
    public bool IsFinished { get; set; }
    public bool HasNewMessage { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; private set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Student> StudentFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected StudentDetailsViewStates States { get; private set; }

    public PopupContext PopupContext { get; set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await this.LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<Student> result)
    {
        switch (result.State)
        {
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
    private async Task<Student> GetStudentAsync()
    {
        var user = await LocalStorage.GetItemAsync<User>("login-user-info");
        if (user == null)
        {
            NavigationManager.NavigateTo("/", true);
            return null;
        }

        var student = await Students.FindAll(x => x.Id == user.Id).Include(x => x.StudentSkills).AsNoTracking().FirstOrDefaultAsync();
        if (student == null)
        {
            NavigationManager.NavigateTo("/", true);
        }

        return student;
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var student = await GetStudentAsync();

            if (student is null)
            {
                this.States = null;
                return;
            }
            IsFinished = student.Stat == Stat.FINISHED;
            this.States = student.ToDetailsViewStates();

            States.StudentSkills = student.StudentSkills.ToList();

            var skills = await Skills.FindAll().AsNoTracking().ToListAsync();
            States.AllSkills = skills;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.StateHasChanged();
        }
    }

    public async void OnLoadSkills()
    {
        var student = await GetStudentAsync();
        if (student is null)
        {
            return;
        }
        States.StudentSkills = student.StudentSkills.ToList();
        StateHasChanged();
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

    public async void OnEditSkillCallback()
    {
        await LoadDataAsync();
    }

    public void OnEditSkill()
    {
        this.PopupContext = new()
        {
            IsOpen = true,
            StudentId = States.Id,
            AllSkills = States.AllSkills,
            OnEditedSkillCallback = OnLoadSkills
        };
    }

    public void OnEdit()
    {
        this.StudentFormRequest = FormRequestFactory.EditRequest(States.ToEntity());
        StateHasChanged();
    }

    public void OnOpenApplyList()
    {
        this.NavigationManager.NavigateTo($"apply-status/{States.Id}");
    }

    public void OnViewResult()
    {

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
