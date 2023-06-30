using System.Collections.ObjectModel;
using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using RCode;
using Syncfusion.Blazor.Inputs;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class ConversationView
{
    #region [ Fields ]
    public SfTextBox? SfTextBox;
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public IConversationRepository Conversations { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public UserManager Users { get; set; }

    [Inject]
    public InstructorManager Instructors { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IMessageRepository Messages { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    #endregion

    #region [ Properties - Parameters ]
    [Parameter]
    public ChatContext Context { get; set; }

    [Parameter]
    public EventCallback LoadOlderMessageCallback { get; set; }

    [Parameter]
    public EventCallback<Message> SendCallback { get; set; }
    #endregion

    #region [ Properties ]
    public ObservableCollection<ChatModel> ChatMessages { get; set; } = new();
    public Message? LastMessage { get; set; }
    protected User Sender { get; set; }
    protected User Receiver { get; set; }
    public string Title { get; set; }
    public string SenderAvatar { get; set; }
    public string ReceiverAvatar { get; set; }
    #endregion

    #region [ Methods - Override ]
    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newContext = parameters.GetValueOrDefault<ChatContext>(nameof(this.Context));
        var currentContext = this.Context;

        await base.SetParametersAsync(parameters);

        if (newContext != null && newContext != currentContext)
        {
            this.LoadDataAsync();
            return;
        }
    }
    #endregion

    #region [ Private Methods - Event Handler ]
    public async void OnLoadOlderMessage()
    {
        await LoadOlderMessageCallback.InvokeAsync();
    }

    public async void OnSend()
    {
        var message = new Message()
        {
            UserId = Sender.Id,
            SentTime = DateTime.Now,
            Content = SfTextBox.Value
        };

        AppendChatMessage(message);
        SfTextBox.Value = "";

        await SendCallback.InvokeAsync(message);
        StateHasChanged();
    }
    #endregion

    #region [ Methods - Data ]
    private async void AppendChatMessage(Message message)
    {
        if(LastMessage == null)
        {
            LastMessage = message;
        }
        else
        {
            if (DateTime.Compare(message.SentTime, LastMessage.SentTime) > 0)
            {
                if ((message.SentTime - LastMessage.SentTime).TotalHours > 1)
                {
                    ChatMessages.Add(
                        new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = "break",
                            CreatedAt = message.SentTime
                        }
                    );
                }
                LastMessage = message;
            }
        }

        var isSender = message.UserId == Sender.Id;
        ChatMessages.Add(new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = isSender ? "Me" : Receiver?.FullName,
            ChatMessage = message.Content,
            CreatedAt = message.SentTime,
            IsSender = isSender,
        });

        await ScrollToEndChat();
    }

    private async void LoadDataAsync()
    {
        try
        {
            Guard.ParamIsNull(Context, nameof(Context));
            Guard.ParamIsNull(Context.Sender, nameof(Context.Sender));
            Guard.ParamIsNull(Context.Receiver, nameof(Context.Receiver));

            Sender = Context.Sender;
            Receiver = Context.Receiver;
            ReceiverAvatar = Context.ReceiverAvatar?? "";
            SenderAvatar = Context.SenderAvatar ?? "";
            Title = Context.ConversationTitle?? "";

            LoadChat();
            await ScrollToEndChat();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            StateHasChanged();
        }

    }

    private void LoadChat()
    {
        ChatMessages.Clear();
        if(Sender != null && Receiver != null)
        {
            Context.Messages.ForEach(x =>
            {
                AppendChatMessage(x);
            });

            LastMessage = Context.Messages.LastOrDefault();
        }
    }

    private async Task ScrollToEndChat()
    {
        
    }
    #endregion
}
