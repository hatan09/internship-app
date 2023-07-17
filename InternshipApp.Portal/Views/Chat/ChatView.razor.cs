using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class ChatView
{
    #region [ Properties - Inject ]
    [Inject]
    public NavigationManager NavigationManager { get; private set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; private set; }

    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public InstructorManager Instructors { get; set; }

    [Inject]
    public RecruiterManager Recruiters { get; private set; }

    [Inject]
    public UserManager Users { get; private set; }

    [Inject]
    public IConversationRepository Conversations { get; private set; }

    [Inject]
    public RoleManager<Role> Roles { get; set; }
    #endregion

    #region [ Properties - States ]
    public bool IsProcessing { get; set; } = true;

    public string ConnectionId { get; set; }

    public User Sender { get; set; }
    public User? Receiver { get; set; }
    public string SenderAvatar { get; set; }
    public string ReceiverAvatar { get; set; }
    public Conversation CurrentConversation { get; set; }
    public List<Message> CurrentMessages { get; set; }

    public bool IsAdminViewing { get; set; }
    public bool IsStudentViewing { get; set; }
    public bool IsTeacherViewing { get; set; }
    public bool IsRecruiterViewing { get; set; }
    #endregion

    #region [ Properties - Contexts ]
    public ChatContext ChatContext { get; set; }
    public ConversationContext ConversationContext { get; set; }
    public HubConnection ConnectionContext;
    #endregion

    #region [ Properties - Data ]
    public Conversation AdminConversation { get; set; }             //ins
    public Conversation InstructorConversation { get; set; }        //student, rec
    public List<Conversation> InstructorConversations { get; set; } //admin
    public List<Conversation> StudentConversations { get; set; }    //ins
    public List<Conversation> RecruiterConversations { get; set; }  //ins

    public List<Student> StudentList { get; set; }
    #endregion

    #region [ Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        CurrentMessages = new();
        InstructorConversations = new();
        StudentConversations = new();
        RecruiterConversations = new();

        ConnectionContext = new HubConnectionBuilder()
                .WithUrl("http://app-internship-app.azurewebsites.net/internship-app-chat")
                .WithAutomaticReconnect()
                .Build();

        RegisterHubCallHandlers();

        await ConnectionContext.StartAsync();

        await base.OnInitializedAsync();
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
            await LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Events - ChatHub ]
    private void RegisterHubCallHandlers()
    {
        ConnectionContext.On<string>("IdentifyUserAsync", this.OnIdentifyUser);
        ConnectionContext.On<int, string, DateTime, string>("IdentifyUserAsync", this.OnReceiveMessage);
    }

    public void OnIdentifyUser(string id)
    {
        ConnectionId = id;
    }

    public void OnReceiveMessage(int conversationId, string fromUserId, DateTime sentTime, string message)
    {
        // Update Chat first to quickly see changes
        // Append Chat => update ChatContext
        if (CurrentConversation.Id == conversationId)
        {
            CurrentMessages.Add(new()
            {
                ConversationId = conversationId,
                UserId = fromUserId,
                Content = message,
                SentTime = sentTime
            });
            ProcessChatContext();
        }

        // Float conversation => update ConversationContext
        UpdateConversationLastMessage(conversationId, sentTime);
        ProcessConversationContext();
    }
    #endregion

    #region [ Methods - Event Handlers ]
    public async void OnLoadOlderMessage()
    {

    }

    public async void OnChat(Message message)
    {
        if (CurrentConversation != null)
        {
            var conversation = await Conversations.FindAll(x => x.Id == CurrentConversation.Id).AsTracking().FirstOrDefaultAsync();
            conversation.Messages.Add(message);
            conversation.LastMessageTime = message.SentTime;
            conversation.LastMessage = message.Content;

            Conversations.Update(conversation);
            await Conversations.SaveChangesAsync();

            UpdateConversationLastMessage(CurrentConversation.Id, message.SentTime);
            ProcessConversationContext();
        }
    }

    public async void OnChangeConversation(int conversationId)
    {
        if (conversationId > 0)
        {
            await LoadCurrentConversation(conversationId);
            await LoadChatAsync();
            ProcessChatContext();
        }
    }

    public async void OnRefresh()
    {

    }
    #endregion

    #region [ Methods - Data ]
    private async Task<User> GetCurrentUserAsync()
    {
        var user = await LocalStorage.GetItemAsync<User>("login-user-info");
        if (user == null)
        {
            NavigationManager.NavigateTo("/", true);
            return null;
        }

        return user;
    }

    private async Task LoadCurrentConversation(int id)
    {
        if (id > 0)
        {
            var conversation = await Conversations.FindAll(x => x.Id == id).Include(x => x.Users).Include(x => x.Messages).AsNoTracking().FirstOrDefaultAsync();
            if (conversation != null)
            {
                CurrentConversation = conversation;
            }
        }
    }

    private async Task LoadChatAsync()
    {
        if (CurrentConversation != null)
        {
            if (CurrentConversation.Users.Where(x => x.Id == Sender.Id).Any())
            {
                Receiver = CurrentConversation.Users.FirstOrDefault(x => x.Id != Sender.Id);
                if (Receiver != null)
                {
                    var student = await Students.FindAll(x => x.Id == Receiver.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (student != null)
                    {
                        ReceiverAvatar = student.ImgUrl;
                    }
                    else
                    {
                        ReceiverAvatar = "";
                    }
                    CurrentMessages = CurrentConversation.Messages.ToList();
                }
            }
        }
    }

    private void UpdateConversationLastMessage(int id, DateTime time)
    {
        if (IsAdminViewing)
        {
            if (InstructorConversations.FirstOrDefault(x => x.Id == id) != null)
            {
                InstructorConversations.FirstOrDefault(x => x.Id == id).LastMessageTime = time;
                InstructorConversations = InstructorConversations.OrderByDescending(x => x.LastMessageTime).ToList();
            }
        }
        else if (IsTeacherViewing)
        {
            if (AdminConversation?.Id == id)
            {
                AdminConversation.LastMessageTime = time;
            }
            else if (StudentConversations.FirstOrDefault(x => x.Id == id) != null)
            {
                StudentConversations.FirstOrDefault(x => x.Id == id).LastMessageTime = time;
                StudentConversations = StudentConversations.OrderByDescending(x => x.LastMessageTime).ToList();
            }
            else if (RecruiterConversations.FirstOrDefault(x => x.Id == id) != null)
            {
                RecruiterConversations.FirstOrDefault(x => x.Id == id).LastMessageTime = time;
                RecruiterConversations = RecruiterConversations.OrderByDescending(x => x.LastMessageTime).ToList();
            }
        }
        else if (IsStudentViewing)
        {
            if (InstructorConversation?.Id == id)
            {
                InstructorConversation.LastMessageTime = time;
            }
        }
        else if (IsRecruiterViewing)
        {
            if (InstructorConversation?.Id == id)
            {
                InstructorConversation.LastMessageTime = time;
            }
        }
    }

    private void ProcessConversationContext()
    {
        var conversationContext = new ConversationContext();

        if (IsAdminViewing)
        {
            InstructorConversations.ForEach(x =>
            {
                conversationContext.InstructorConversations.Add(
                    x.ToListRow(receiverName: x.Users.FirstOrDefault(x => x.Id != Sender.Id)?.FullName));
            });
        }
        else if (IsTeacherViewing)
        {
            conversationContext.AdminConversation = AdminConversation?.ToListRow(receiverName: "Administrator");

            StudentConversations.ForEach(x =>
            {
                conversationContext.StudentConversations.Add(
                    x.ToListRow(
                        receiverAvatarUrl: StudentList.FirstOrDefault(y => y.Id == x.Users.FirstOrDefault(x => x.Id != Sender.Id)?.Id)?.ImgUrl,
                        receiverName: x.Users.FirstOrDefault(x => x.Id != Sender.Id)?.FullName));
            });

            RecruiterConversations.ForEach(x =>
            {
                conversationContext.RecruiterConversations.Add(
                    x.ToListRow(receiverName: x.Users.FirstOrDefault(x => x.Id != Sender.Id)?.FullName));
            });
        }
        else if (IsStudentViewing)
        {
            conversationContext.InstructorConversation = InstructorConversation?.ToListRow(receiverName: InstructorConversation.Users.FirstOrDefault(x => x.Id != Sender.Id)?.FullName);
        }
        else if (IsRecruiterViewing)
        {
            conversationContext.InstructorConversation = InstructorConversation?.ToListRow(receiverName: InstructorConversation.Users.FirstOrDefault(x => x.Id != Sender.Id)?.FullName);
        }

        ConversationContext = conversationContext;
        StateHasChanged();
    }

    private void ProcessChatContext()
    {
        var context = new ChatContext()
        {
            Sender = Sender,
            SenderAvatar = SenderAvatar,
            Receiver = Receiver,
            ReceiverAvatar = ReceiverAvatar,
            ConversationTitle = CurrentConversation.Title,
            Messages = CurrentMessages
        };

        ChatContext = context;
        StateHasChanged();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            IsProcessing = true;
            var user = await GetCurrentUserAsync();
            Sender = user;

            await ConnectionContext.InvokeAsync("OnConnectedAsync");

            await ConnectionContext.InvokeAsync("ChatHubUserIndentity", ConnectionId, user.Id);

            if (IsAdminViewing)
            {
                var instructorRole = await Roles.FindByNameAsync("instructor");

                var adminConversations = await Conversations
                    .FindAll(x => x.Users.Where(x => x.Id == user.Id).Any())
                    .Include(x => x.Users)
                    .ThenInclude(x => x.UserRoles)
                    .AsNoTracking().ToListAsync();
                var admin_insConversations = adminConversations
                                                    .Where(x => x.Users
                                                        .Where(x => x.UserRoles
                                                            .Where(x => x.RoleId == instructorRole.Id).Any())
                                                        .Any()).OrderByDescending(x => x.LastMessageTime).ToList();
                InstructorConversations = admin_insConversations;
            }
            else if (IsTeacherViewing)
            {
                var adminRole = await Roles.FindByNameAsync("admin");
                var studentRole = await Roles.FindByNameAsync("student");
                var recruiterRole = await Roles.FindByNameAsync("recruiter");

                var instructorConversations = await Conversations.FindAll(x => x.Users.Where(x => x.Id == user.Id).Any()).Include(x => x.Users).ThenInclude(x => x.UserRoles).AsNoTracking().ToListAsync();
                var ins_adminConversation = instructorConversations
                                                    .Where(x => x.Users
                                                        .Where(x => x.UserRoles
                                                            .Where(x => x.RoleId == adminRole.Id).Any())
                                                        .Any()).FirstOrDefault();
                AdminConversation = ins_adminConversation;

                var ins_studentConversations = instructorConversations
                                                    .Where(x => x.Users
                                                        .Where(x => x.UserRoles
                                                            .Where(x => x.RoleId == studentRole.Id).Any())
                                                        .Any()).OrderByDescending(x => x.LastMessageTime).ToList();
                StudentConversations = ins_studentConversations;

                var ins_recruiterConversations = instructorConversations
                                                    .Where(x => x.Users
                                                        .Where(x => x.UserRoles
                                                            .Where(x => x.RoleId == recruiterRole.Id).Any())
                                                        .Any()).OrderByDescending(x => x.LastMessageTime).ToList();
                RecruiterConversations = ins_recruiterConversations;

                //load student's avatar
                var studentIds = new List<string>();
                foreach (var x in StudentConversations.Select(x => x.Users))
                {
                    studentIds.AddRange(x.Select(x => x.Id));
                }

                var students = await Students.FindAll(x => studentIds.Contains(x.Id)).Select(x => new Student()
                {
                    Id = x.Id,
                    ImgUrl = x.ImgUrl,
                }).ToListAsync();
                if (students != null && students.Count > 0)
                {
                    StudentList = students;
                }
            }
            else if (IsStudentViewing)
            {
                var student = await Students.FindAll(x => x.Id == user.Id)
                                            .Include(x => x.InternGroup)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();
                if (student != null && student.InternGroup != null)
                {
                    SenderAvatar = student.ImgUrl;
                    var instructorId = student.InternGroup.InstructorId;

                    var studentConversations = await Conversations.FindAll(x => x.Users.Where(x => x.Id == user.Id).Any()).Include(x => x.Users).AsNoTracking().ToListAsync();
                    var student_insConversation = studentConversations.Where(x => x.Users.Where(x => x.Id == instructorId).Any()).FirstOrDefault();
                    InstructorConversation = student_insConversation;
                }
            }
            else if (IsRecruiterViewing)
            {
                var instructor = await Instructors.FindAll(x => x.IsDepartmentManager).AsTracking().FirstOrDefaultAsync();
                if (instructor != null)
                {
                    var recruiterConversations = await Conversations.FindAll(x => x.Users.Where(x => x.Id == user.Id).Any()).Include(x => x.Users).AsNoTracking().ToListAsync();
                    var recruiter_insConversation = recruiterConversations.Where(x => x.Users.Where(x => x.Id == instructor.Id).Any()).FirstOrDefault();
                    if (recruiter_insConversation == null)
                    {
                        var recruiter = await Recruiters.FindAll(x => x.Id == user.Id).AsTracking().FirstOrDefaultAsync();
                        recruiter_insConversation = new()
                        {
                            Title = $"{instructor.FullName}_{recruiter.FullName} Conversation",
                            Users =
                            {
                                recruiter,
                                instructor
                            },

                        };
                        Conversations.Add(recruiter_insConversation);
                        await Conversations.SaveChangesAsync();
                    }
                    InstructorConversation = recruiter_insConversation;
                }
            }
            await ConnectionContext.InvokeAsync("JoinAllConversations");
            OnInitializeContext();
        }
        catch (Exception)
        {

        }
        finally
        {
            IsProcessing = false;
            StateHasChanged();
        }
    }

    private void OnInitializeContext()
    {
        ProcessConversationContext();
        ChatContext = new();
    }
    #endregion
}
