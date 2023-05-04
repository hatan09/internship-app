using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class CommandBarView
{
    [Parameter]
    public EventCallback EditCallback { get; set; }

    [Parameter]
    public EventCallback ChatCallback { get; set; }

    [Parameter]
    public EventCallback SendEmailCallback { get; set; }

    [Parameter]
    public EventCallback AcceptCallback { get; set; }

    [Parameter]
    public EventCallback RejectCallback { get; set; }

    [Parameter]
    public EventCallback ApplyListCallback { get; set; }

    public bool IsEditButtonVisible { get; set; }
    public bool IsChatButtonVisible { get; set; }
    public bool IsSendEmailButtonVisible { get; set; }
    public bool IsAcceptButtonVisible { get; set; }
    public bool IsRejectButtonVisible { get; set; }
    public bool IsApplyListButtonVisible { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsEditButtonVisible = EditCallback.HasDelegate;
        IsChatButtonVisible = ChatCallback.HasDelegate;
        IsSendEmailButtonVisible = SendEmailCallback.HasDelegate;
        IsAcceptButtonVisible = AcceptCallback.HasDelegate;
        IsRejectButtonVisible = RejectCallback.HasDelegate;
        IsApplyListButtonVisible = ApplyListCallback.HasDelegate;

        await base.OnInitializedAsync();
    }
}
