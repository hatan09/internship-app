using System.Collections.ObjectModel;
using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Inputs;

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
    public string ReceiverId { get; set; }
    #endregion

    #region [ Properties ]
    private Conversation Conversation { get; set; }
    private Message LastMessage { get; set; }
    public ObservableCollection<ChatModel> Chat { get; set; } = new();
    protected string Role { get; set; }
    protected User Sender { get; set; }
    protected User Receiver { get; set; }

    public string SenderAvatar { get; set; }

    public string ReceiverAvatar { get; set; }
    #endregion

    #region [ Methods - Override ]
    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var role = await LocalStorage.GetItemAsStringAsync("role");
            if (!string.IsNullOrEmpty(role) && role != "ADMIN" && role != "RECRUITER")
            {
                Role = role;
                await LoadDataAsync();
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Private Methods - Event Handler ]
    public async Task OnSend()
    {
        if (Conversation == null)
        {
            var sender = await Users.FindAll(x => x.Id == Sender.Id).AsTracking().FirstOrDefaultAsync();
            var receiver = await Users.FindAll(x => x.Id == Receiver.Id).AsTracking().FirstOrDefaultAsync();
            var newConversation = new Conversation()
            {
                Title = Guid.NewGuid().ToString(),
                Users = new[]
                {
                    receiver,
                    sender
                },
            };

            Conversations.Add(newConversation);
            newConversation.Messages.Add(new Message()
            {
                Conversation = newConversation,
                UserId = Sender.Id,
                SentTime = DateTime.Now,
                Content = SfTextBox.Value
            });

            Conversation = newConversation;
        }
        else
        {
            Conversation.Messages.Add(new Message()
            {
                Conversation = Conversation,
                UserId = Sender.Id,
                SentTime = DateTime.Now,
                Content = SfTextBox.Value
            });
        }

        await Conversations.SaveChangesAsync();
        var message = Conversation.Messages.Last();
        AppendChat(message);
        SfTextBox.Value = "";
        StateHasChanged();
    }
    #endregion

    #region [ Methods - Data ]
    private void AppendChat(Message message)
    {
        if (DateTime.Compare(message.SentTime, LastMessage.SentTime) > 0)
        {
            if ((message.SentTime - LastMessage.SentTime).TotalHours > 1)
            {
                Chat.Add(
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

        var isSender = message.UserId == Sender.Id;
        Chat.Add(new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = isSender ? "Me" : Receiver?.FullName,
            ChatMessage = message.Content,
            CreatedAt = message.SentTime,
            IsSender = isSender,
        });
    }

    private async Task LoadChatAsync()
    {
        //var sender = await Users.FindAll(x => x.Id == Sender.Id).Include(x => x.Conversations).ThenInclude(x => x.Users).FirstOrDefaultAsync();
        //var convsersation = sender.Conversations.FirstOrDefault(x => x.Users.Contains(Receiver));

        Conversation = await Conversations
            .FindAll(x => x.Users.Contains(Sender) && x.Users.Contains(Receiver))
            .AsTracking()
            .Include(x => x.Messages.OrderBy(x => x.SentTime))
            .FirstOrDefaultAsync();

        #region [ Samples ]
        Chat.Add(
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    IsSender = Role == "STUDENT",
                    Name = Role == "STUDENT" ? "Me" : Receiver?.FullName,
                    ChatMessage = "Hello Mr. Sinh. I have a few questions to ask. Please tell me when you have free time.",
                    CreatedAt = DateTime.Parse("12/12/22")
                }
            );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "INSTRUCTOR",
                Name = Role == "INSTRUCTOR" ? "Me" : "Nguyen Van Sinh",
                ChatMessage = "Hello Tan,\nWhat can I help you?",
                CreatedAt = DateTime.Parse("12/12/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "STUDENT",
                Name = Role == "STUDENT" ? "Me" : Receiver?.FullName,
                ChatMessage = "Thank you for replying.",
                CreatedAt = DateTime.Parse("12/12/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "STUDENT",
                Name = Role == "STUDENT" ? "Me" : Receiver?.FullName,
                ChatMessage = "I'm having issue in applying for jobs because I can't, and all apply buttons say \"can't apply\"",
                CreatedAt = DateTime.Parse("12/12/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "INSTRUCTOR",
                Name = Role == "INSTRUCTOR" ? "Me" : "Nguyen Van Sinh",
                ChatMessage = "Don't worry, it's because you have not been added to any intern group yet",
                CreatedAt = DateTime.Parse("12/12/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "INSTRUCTOR",
                Name = Role == "INSTRUCTOR" ? "Me" : "Nguyen Van Sinh",
                ChatMessage = "The IT department is still working with partnership companies to find jobs and hold workshops. You can see more information here: https://iujobhub.com/",
                CreatedAt = DateTime.Parse("12/12/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "STUDENT",
                Name = Role == "STUDENT" ? "Me" : Receiver?.FullName,
                ChatMessage = "I got it. Thank you for your help!",
                CreatedAt = DateTime.Parse("12/12/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "INSTRUCTOR",
                Name = Role == "INSTRUCTOR" ? "Me" : "Nguyen Van Sinh",
                ChatMessage = "You're welcome!",
                CreatedAt = DateTime.Parse("12/12/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Parse("5/25/23"),
                Name = "break"
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "INSTRUCTOR",
                Name = Role == "INSTRUCTOR" ? "Me" : "Nguyen Van Sinh",
                ChatMessage = "Hi, Tan. I'm reminding you that you haven't submitted the internship report. Please do it before 1-6-2023",
                CreatedAt = DateTime.Parse("5/25/22")
            }
        );
        Chat.Add(
            new()
            {
                Id = Guid.NewGuid().ToString(),
                IsSender = Role == "STUDENT",
                Name = Role == "STUDENT" ? "Me" : Receiver?.FullName,
                ChatMessage = "Thank you! I'm having it signed by company and supervisor. I expect it to be done before May 29th.",
                CreatedAt = DateTime.Parse("5/25/22")
            }
        );
        #endregion

        LastMessage = new()
        {
            Id = 10,
            Content = "Thank you! I'm having it signed by company and supervisor. I expect it to be done before May 29th.",
            SentTime = DateTime.Parse("5/25/22")
        };

        if (Conversation != null)
        {
            var allMessage = Conversation.Messages.ToList();

            foreach (var message in allMessage)
            {
                AppendChat(message);
            }
        }
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var sender = await LocalStorage.GetItemAsync<User>("login-user-info");
            if (sender == null)
            {
                NavigationManager.NavigateTo("/", true);
                return;
            }
            Sender = sender;

            if (Role == "STUDENT")
            {
                var student = await Students.FindByIdAsync(sender.Id);
                if (student == null)
                {
                    return;
                }

                SenderAvatar = student.ImgUrl;
            }

            if (ReceiverId == "student")
            {
                if (Role != "STUDENT")
                {
                    return;
                }

                var student = await Students.FindAll(x => x.Id == sender.Id)
                    .AsNoTracking()
                    .Include(x => x.InternGroup)
                    .ThenInclude(x => x.Instructor)
                    .FirstOrDefaultAsync();
                if (student == null || student.InternGroup == null || student.InternGroup.Instructor == null)
                {
                    return;
                }

                //ReceiverAvatar = student.InternGroup.Instructor;
                Receiver = student.InternGroup.Instructor;
            }
            else
            {
                var student = await Students.FindByIdAsync(ReceiverId);
                if (student == null)
                {
                    return;
                }

                ReceiverAvatar = student.ImgUrl;
                Receiver = student as User;
            }


            await LoadChatAsync();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            StateHasChanged();

        }

    }
    #endregion
}
