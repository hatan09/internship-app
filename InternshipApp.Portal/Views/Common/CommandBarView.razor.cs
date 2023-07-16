using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class CommandBarView
{
    #region [ Properties - Param ]
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

    [Parameter]
    public EventCallback ShowScoreCallback { get; set; }

    [Parameter]
    public EventCallback FinishCallback { get; set; }

    [Parameter]
    public EventCallback ViewEvaluationCallback { get; set; }

    [Parameter]
    public EventCallback ViewFinalFormsCallback { get; set; }

    [Parameter]
    public EventCallback ViewResultCallback { get; set; }

    [Parameter]
    public EventCallback AddGroupCallback { get; set; }

    [Parameter]
    public EventCallback UndoCallback { get; set; }
    #endregion

    #region [ Properties ]
    public bool IsEditButtonVisible { get; set; }
    public bool IsChatButtonVisible { get; set; }
    public bool IsSendEmailButtonVisible { get; set; }
    public bool IsAcceptButtonVisible { get; set; }
    public bool IsRejectButtonVisible { get; set; }
    public bool IsApplyListButtonVisible { get; set; }
    public bool IsShowScoreButtonVisible { get; set; }
    public bool IsFinishButtonVisible { get; set; }
    public bool IsViewEvaluationButtonVisible { get; set; }
    public bool IsViewFinalFormsVisible { get; set; }
    public bool IsViewResultButtonVisible { get; set; }
    public bool IsAddGroupButtonVisible { get; set; }
    public bool IsUndoButtonVisible { get; set; }
    #endregion

    #region [ Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        IsEditButtonVisible = EditCallback.HasDelegate;
        IsChatButtonVisible = ChatCallback.HasDelegate;
        IsSendEmailButtonVisible = SendEmailCallback.HasDelegate;
        IsAcceptButtonVisible = AcceptCallback.HasDelegate;
        IsRejectButtonVisible = RejectCallback.HasDelegate;
        IsApplyListButtonVisible = ApplyListCallback.HasDelegate;
        IsShowScoreButtonVisible = ShowScoreCallback.HasDelegate;
        IsFinishButtonVisible = FinishCallback.HasDelegate;
        IsViewEvaluationButtonVisible = ViewEvaluationCallback.HasDelegate;
        IsViewFinalFormsVisible = ViewFinalFormsCallback.HasDelegate;
        IsViewResultButtonVisible = ViewResultCallback.HasDelegate;
        IsAddGroupButtonVisible = AddGroupCallback.HasDelegate;
        IsUndoButtonVisible = UndoCallback.HasDelegate;

        await base.OnInitializedAsync();
    }
    #endregion
}
