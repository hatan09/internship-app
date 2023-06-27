using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public class ConversationContext
{
    #region [ CTor ]
    public ConversationContext()
    {
        InstructorConversations = new();
        StudentConversations = new();
        RecruiterConversations = new();
    }
    #endregion

    #region [ Properties ]
    public User Sender { get; set; }
    public ConversationListRowViewStates AdminConversation { get; set; }
    public ConversationListRowViewStates InstructorConversation { get; set; }
    public List<ConversationListRowViewStates> InstructorConversations { get; set; }
    public List<ConversationListRowViewStates> StudentConversations { get; set; }
    public List<ConversationListRowViewStates> RecruiterConversations { get; set; }
    #endregion
}
