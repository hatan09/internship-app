using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public class ChatContext
{
    public User Sender { get; set; }
    public User Receiver { get; set; }
    public string ConversationTitle { get; set; }
    public string Avatar { get; set; }
    public List<Message> Messages { get; set; }
}
