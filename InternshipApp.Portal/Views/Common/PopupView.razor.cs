using Blazored.LocalStorage;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class PopupView
{
    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Parameter]
    public bool IsSendEmailView { get; set; }

    [Parameter]
    public bool IsEditSkillView { get; set; }

    [Parameter]
    public bool IsShowScoreView { get; set; }

    [Parameter]
    public bool IsShowResultView { get; set; }

    [Parameter]
    public bool IsNotifyView { get; set; }

    [Parameter]
    public PopupContext Context { get; set; }

    public bool Visible { get; set; }

    private bool IsSkillEdited { get; set; } = false;

    private bool IsTeacherViewing { get; set; } = false;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newContext = parameters.GetValueOrDefault<PopupContext>(nameof(this.Context));
        var currentContext = this.Context;

        await base.SetParametersAsync(parameters);
        if (newContext != null && newContext != currentContext)
        {
            Visible = Context.IsOpen;
            IsSkillEdited = false;
            StateHasChanged();

        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            var role = await LocalStorage.GetItemAsStringAsync("role");
            switch(role)
            {
                case "INSTRUCTOR":
                    IsTeacherViewing = true;
                    break;
                default:
                    break;
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public void OnSkillEdit()
    {
        IsSkillEdited = true;
    }

    public void OnCloseHandler()
    {
        if(IsEditSkillView && IsSkillEdited)
        {
            Context.OnEditedSkillCallback?.Invoke();
            IsSkillEdited = false;
        }
    }

    public async void OnFinishButtonClicked()
    {
        if (IsTeacherViewing)
        {
            await Context.OnFinishCallBack.InvokeAsync();
        }
    }
}

public class PopupContext
{
    public bool IsOpen { get; set; }
    public string Message { get; set; }
    public string StudentId { get; set; }
    public string CompanyName { get; set; }
    public List<Skill> AllSkills { get; set; }
    public Action OnEditedSkillCallback { get; set; }
    public EventCallback OnFinishCallBack { get; set; }
}
