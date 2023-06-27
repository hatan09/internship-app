using Blazored.LocalStorage;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class ConversationListView
{
    #region [ Properties - Param ]
    [Parameter]
    public ConversationContext Context { get; set; }

    [Parameter]
    public EventCallback<int> SelectConversationCallback { get; set; }

    [Parameter]
    public EventCallback RefreshCallback { get; set; }
    #endregion

    #region [ Properties ]
    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    #endregion

    #region [ Properties ]
    public User Sender { get; set; }
    private bool HasConversation { get; set; }
    public ConversationListRowViewStates AdminConversation { get; set; }
    public ConversationListRowViewStates InstructorConversation { get; set; }
    public List<ConversationListRowViewStates> InstructorConversations { get; set; }
    public List<ConversationListRowViewStates> StudentConversations { get; set; }
    public List<ConversationListRowViewStates> RecruiterConversations { get; set; }
    public bool IsAdminViewing { get; set; }
    public bool IsStudentViewing { get; set; }
    public bool IsTeacherViewing { get; set; }
    public bool IsRecruiterViewing { get; set; }
    #endregion

    #region [ Methods - Override ]
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newContext = parameters.GetValueOrDefault<ConversationContext>(nameof(this.Context));
        var currentContext = this.Context;

        await base.SetParametersAsync(parameters);

        if (newContext != null && newContext != currentContext)
        {
            LoadData();
            return;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var role = await LocalStorage.GetItemAsStringAsync("role");
            switch (role)
            {
                case "ADMIN":
                    IsAdminViewing = true;
                    break;
                case "STUDENT":
                    IsStudentViewing = true;
                    break;
                case "INSTRUCTOR":
                    IsTeacherViewing = true;
                    break;
                case "RECRUITER":
                    IsRecruiterViewing = true;
                    break;
                default:
                    break;
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Methods - Event Handlers ]
    public async void OnSelect(ConversationListRowViewStates selectedItem)
    {
        if (selectedItem != null && selectedItem.Id > 0)
        {
            await SelectConversationCallback.InvokeAsync(selectedItem.Id);
            return;
        }
    }
    #endregion

    #region [ Methods - Data ]
    private void LoadData()
    {
        Sender = Context.Sender;
        if(Sender == null)
        {
            return;
        }

        if (IsAdminViewing)
        {
            InstructorConversations = Context.InstructorConversations;
            HasConversation = InstructorConversations.Count > 0;
        }
        else if (IsTeacherViewing)
        {
            AdminConversation = Context.AdminConversation;
            StudentConversations = Context.StudentConversations;
            RecruiterConversations = Context.RecruiterConversations;
            HasConversation = AdminConversation != null || StudentConversations.Count > 0 || RecruiterConversations.Count > 0;
        }
        else if (IsStudentViewing)
        {
            InstructorConversation = Context.InstructorConversation;
            HasConversation = InstructorConversation != null;
        }
        else if (IsRecruiterViewing)
        {
            InstructorConversation = Context.InstructorConversation;
            HasConversation = InstructorConversation != null;
        }
    }
    #endregion
}
