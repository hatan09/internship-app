using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public static class ConversationExtensions
{
    #region [ Public Methods - ListRow ]
    public static List<ConversationListRowViewStates> ToListRowList(this List<Conversation> list, string receiverAvatarUrl = "", string receiverName = "")
    {
        var result = new List<ConversationListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow(receiverAvatarUrl, receiverName)));
        return result;
    }

    public static ConversationListRowViewStates ToListRow(this Conversation entity, string receiverAvatarUrl = "", string receiverName = "")
    {

        var viewStates = new ConversationListRowViewStates()
        {
            Id = entity.Id,
            Avatar = receiverAvatarUrl,
            LastMsg = entity.LastMessage,
            LastMsgTime = entity.LastMessageTime?? DateTime.Now,
            Name = receiverName
        };

        return viewStates;
    }
    #endregion 
}
