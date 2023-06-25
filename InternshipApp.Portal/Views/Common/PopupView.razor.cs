using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class PopupView
{
    [Parameter]
    public bool IsSendEmailView { get; set; }

    [Parameter]
    public bool IsEditSkillView { get; set; }

    [Parameter]
    public bool IsShowScoreView { get; set; }

    [Parameter]
    public bool IsNotifyView { get; set; }

    [Parameter]
    public PopupContext Context { get; set; }

    public bool Visible { get; set; }

    private bool IsSkillEdited { get; set; } = false;

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

    public void OnSkillEdit()
    {
        IsSkillEdited = true;
    }

    public async void OnCloseHandler()
    {
        if(IsEditSkillView && IsSkillEdited)
        {
            if (Context.OnEditSkillCallback.HasDelegate) await Context.OnEditSkillCallback.InvokeAsync();
        }
    }

    public async void OnFinishButtonClicked()
    {
        await Context.OnFinishCallBack.InvokeAsync();
    }
}

public class PopupContext
{
    public bool IsOpen { get; set; }
    public string Message { get; set; }
    public string StudentId { get; set; }
    public string CompanyName { get; set; }
    public List<Skill> AllSkills { get; set; }
    public EventCallback OnEditSkillCallback { get; set; }
    public EventCallback OnFinishCallBack { get; set; }
}
