﻿using Microsoft.AspNetCore.Components;

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
    public EventCallback ShowScoreCallBack { get; set; }

    [Parameter]
    public EventCallback FinishCallback { get; set; }

    [Parameter]
    public EventCallback ViewEvaluationCallBack { get; set; }

    [Parameter]
    public EventCallback ViewResultCallBack { get; set; }

    [Parameter]
    public EventCallback AddGroupCallBack { get; set; }

    [Parameter]
    public EventCallback UndoCallBack { get; set; }
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
        IsShowScoreButtonVisible = ShowScoreCallBack.HasDelegate;
        IsFinishButtonVisible = FinishCallback.HasDelegate;
        IsViewEvaluationButtonVisible = ViewEvaluationCallBack.HasDelegate;
        IsViewResultButtonVisible = ViewResultCallBack.HasDelegate;
        IsAddGroupButtonVisible = AddGroupCallBack.HasDelegate;
        IsUndoButtonVisible = UndoCallBack.HasDelegate;

        await base.OnInitializedAsync();
    }
    #endregion
}
