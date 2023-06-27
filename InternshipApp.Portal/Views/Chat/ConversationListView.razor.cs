using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class ConversationListView
{
    #region [ Properties - Param ]
    [Parameter]
    public ConversationContext Context { get; set; }

    [Parameter]
    public EventCallback<string> SelectConversationCallback { get; set; }

    [Parameter]
    public EventCallback RefreshCallback { get; set; }
    #endregion

    #region [ Properties ]
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
    #endregion

    #region [ Methods - Event Handlers ]
    public async void OnSelect(ConversationListRowViewStates selectedItem)
    {

    }
    #endregion

    #region [ Methods - Data ]
    private void LoadData()
    {
        if (IsAdminViewing)
        {
            InstructorConversations = Context.InstructorConversations;
        }
    }
    #endregion
}
