using AutoMapper;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Hubs;

[AllowAnonymous]
public class ChatHub : Hub
{
    private IMapper _mapper;
    private UserManager _userManager;
    private IMessageRepository _messageRepository;
    private IConversationRepository _conversationRepository;

    public ChatHub(IMapper mapper,
                   UserManager userManager,
                   IMessageRepository messageRepository,
                   IConversationRepository conversationRepository)
    {
        _mapper = mapper;
        _userManager = userManager;
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("IdentifyUser", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var token = new CancellationToken(default);
        var user = await _userManager.FindBySignalRConnectionId(Context.ConnectionId);
        if (user != null)
        {
            user.SignalRConnectionId = null;
            await _userManager.UpdateAsync(user);
            await Clients.All.SendAsync("UserLogOut", Context.ConnectionId);
            if (user.Conversations != null && user.Conversations.Any())
            {
                foreach (var conversation in user.Conversations)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversation.Id.ToString(), token);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinConversation(int conversationId)
    {
        var token = new CancellationToken(default);
        var conversation = await _conversationRepository.FindByIdAsync(conversationId, token);
        if (conversation != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString());
        }
    }

    public async Task JoinAllConversations()
    {
        var token = new CancellationToken(default);
        var user = await _userManager
            .FindAll(x => x.SignalRConnectionId != null && x.SignalRConnectionId == Context.ConnectionId)
            .Include(x => x.Conversations)
            .FirstOrDefaultAsync(token);
        if (user != null && user.Conversations != null && user.Conversations.Any())
        {
            foreach (var conversation in user.Conversations)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString());
            };
        }
    }

    public async Task ChatHubUserIndentity(string connectionId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        user.SignalRConnectionId = connectionId;
        await _userManager.UpdateAsync(user);
    }

    //public async Task OnlineUsersListChange()
    //{
    //    await Clients.All.SendAsync("UsersListChange", StaticUserList.SignalROnlineUsers);
    //}

    public async Task SendMessage(string mess,
                                  DateTime sentTime,
                                  int conversationId,
                                  string fromUserId,
                                  string toUserId)
    {
        var cancellationToken = new CancellationToken(default);
        var conversation = await _conversationRepository.FindByIdAsync(conversationId, cancellationToken);
        if (conversation != null)
        {
            var fromUser = await _userManager.FindByIdAsync(fromUserId);
            var toUser = await _userManager.FindByIdAsync(toUserId);

            //VietNam Time
            //var VietNamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            //var dt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            //var DateTimeInVietNamLocal = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, VietNamZone);

            _messageRepository.Add(new Message()
            {
                User = fromUser,
                Content = mess,
                Conversation = conversation,
                SentTime = sentTime
            });
            conversation.LastMessage = mess;
            conversation.LastMessageTime = DateTime.Now;
            _conversationRepository.Update(conversation);
            await _messageRepository.SaveChangesAsync(cancellationToken);
            await _conversationRepository.SaveChangesAsync(cancellationToken);
            if (toUser.SignalRConnectionId != null)
            {
                await Clients.Client(toUser.SignalRConnectionId).SendAsync("ReceiveMessage", conversationId, fromUserId, mess, sentTime);
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("The conversation does not exist");
        }
    }
}