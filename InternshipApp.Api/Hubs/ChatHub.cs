using AutoMapper;
using InternshipApp.Repository;
using Microsoft.AspNetCore.SignalR;

namespace InternshipApp.Hubs;

public class ChatHub : Hub
{
    private IMapper _mapper;
    private UserManager _userManager;
    //IMessageRepository _messageRepository;
    //private IConversationRepository _conversationRepository;

    public ChatHub(IMapper mapper,
                   UserManager userManager
                   //IMessageRepository messageRepository,
                   //IConversationRepository conversationRepository)
                   )
    {
        _mapper = mapper;
        _userManager = userManager;
        //_messageRepository = messageRepository;
        //_conversationRepository = conversationRepository;
    }

    //public override async Task OnConnectedAsync()
    //{
    //    System.Diagnostics.Debug.WriteLine(Context.ConnectionId);
    //    var allOnlineUsers = _userManager
    //                                .FindAll(user => user.SignalRConnectionId != null)
    //                                .ToList();
    //    await Clients.Caller.SendAsync("IdentifyUser", Context.ConnectionId, _mapper.Map<IEnumerable<UserDTO>>(allOnlineUsers));
    //    await Clients.All.SendAsync("ReceiveMessage", $"Welcome {Context.ConnectionId}");
    //    await base.OnConnectedAsync();
    //}

    //public override async Task OnDisconnectedAsync(Exception exception)
    //{
    //    CancellationToken cancellationToken = new CancellationToken(default);
    //    var user = await _userManager.FindBySignalRConnectionId(Context.ConnectionId);
    //    if (user != null)
    //    {
    //        user.SignalRConnectionId = null;
    //        _userManager.Update(user);
    //        await _userManager.SaveChangesAsync(cancellationToken);
    //        await Clients.All.SendAsync("UserLogOut", Context.ConnectionId);
    //    }

    //    await base.OnDisconnectedAsync(exception);
    //}

    //public async Task JoinGroup(int groupId, int userId)
    //{
    //    CancellationToken token = new CancellationToken(default);
    //    var user = await _userManager.FindByIdAsync(userId, token);
    //    var group = await _groupChatRepository.FindByIdAsync(groupId, token);
    //    await Groups.AddToGroupAsync(Context.ConnectionId, group.GroupName);
    //}

    //public async Task ChatHubUserIndentity(string connectionId, int userId)
    //{
    //    CancellationToken cancellationToken = new CancellationToken(default);
    //    var user = await _userManager.FindByIdAsync(userId, cancellationToken);
    //    user.SignalRConnectionId = connectionId;
    //    _userManager.Update(user);
    //    await _userManager.SaveChangesAsync(cancellationToken);
    //    await Clients.Client(connectionId).SendAsync("ChatHubUserIndentity",
    //                                                 _mapper.Map<UserDTO>(user));
    //    await Clients.All.SendAsync("UserLogIn", _mapper.Map<UserDTO>(user));
    //}

    ////public async Task OnlineUsersListChange()
    ////{
    ////    await Clients.All.SendAsync("UsersListChange", StaticUserList.SignalROnlineUsers);
    ////}

    //public async Task SendMessage(string mess,
    //                              DateTime sentTime,
    //                              int conversationId,
    //                              int fromUserId,
    //                              int toUserId)
    //{
    //    CancellationToken cancellationToken = new CancellationToken(default);
    //    var conversation = await _conversationRepository.FindByIdAsync(conversationId, cancellationToken);
    //    if (conversation != null)
    //    {
    //        var fromUser = await _userManager.FindByIdAsync(fromUserId, cancellationToken);
    //        var toUser = await _userManager.FindByIdAsync(toUserId, cancellationToken);

    //        //VietNam Time
    //        //var VietNamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
    //        //var dt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    //        //var DateTimeInVietNamLocal = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, VietNamZone);

    //        _messageRepository.Create(new ChatMessage()
    //        {
    //            User = fromUser,
    //            MessageContent = mess,
    //            Conversation = conversation,
    //            SentTime = sentTime
    //        });
    //        conversation.LastMessageContent = mess;
    //        conversation.LastInteractionTime = DateTime.UtcNow;
    //        _conversationRepository.Update(conversation);
    //        await _messageRepository.SaveChangesAsync(cancellationToken);
    //        await _conversationRepository.SaveChangesAsync(cancellationToken);
    //        if (toUser.SignalRConnectionId != null)
    //        {
    //            await Clients.Client(toUser.SignalRConnectionId).SendAsync("ReceiveMessage", mess, sentTime, _mapper.Map<UserDTO>(fromUser));
    //        }

    //        await Clients.Caller.SendAsync("ReceiveMessage", mess, sentTime, _mapper.Map<UserDTO>(fromUser));
    //    }
    //    else
    //    {
    //        System.Diagnostics.Debug.WriteLine("The conversation does not exist");
    //    }
    //}
}