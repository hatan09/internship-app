using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class ConversationListView
{
    #region [ Properties - Param ]
    [Parameter]
    public ConversationContext Context { get; set; }

    [Parameter]
    public EventCallback<string> SelectConversationCallback { get; set; }
    #endregion

    #region [ Properties ]
    public List<ConversationListRowViewStates> StudentConversations { get; set; }
    public List<ConversationListRowViewStates> RecruiterConversations { get; set; }
    public bool IsAdminViewing { get; set; }
    public bool IsStudentViewing { get; set; }
    public bool IsTeacherViewing { get; set; }
    public bool IsRecruiterViewing { get; set; }
    #endregion

    #region [ Methods -  ]
    public override Task SetParametersAsync(ParameterView parameters)
    {
        return base.SetParametersAsync(parameters);
    }
    #endregion

    #region [ Methods - Event Handlers ]
    public async void OnSelect(ConversationListRowViewStates selectedItem)
    {

    }
    #endregion
}
