using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class PopupView
{
    [Parameter]
    public bool IsSendEmailView { get; set; }

    [Parameter]
    public bool IsEditSkillView { get; set; }

    [Parameter]
    public string StudentId { get; set; }

    [Parameter]
    public PopupContext Context { get; set; }

    public bool Visible { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newContext = parameters.GetValueOrDefault<PopupContext>(nameof(this.Context));
        var currentContext = this.Context;

        await base.SetParametersAsync(parameters);
        if (newContext != null && newContext != currentContext)
        {
            Visible = Context.IsOpen;
            StateHasChanged();

        }
    }
}

public class PopupContext
{
    public bool IsOpen { get; set; }
}
