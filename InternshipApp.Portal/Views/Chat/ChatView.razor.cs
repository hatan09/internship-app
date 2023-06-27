﻿using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
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
    public UserManager Users { get; private set; }

    [Inject]
    public IConversationRepository Conversations { get; private set; }

    [Inject]
    public RoleManager<Role> Roles { get; set; }
    #endregion

    #region [ Properties - States ]
    public User Sender { get; set; }
    public User? Receiver { get; set; }
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
    protected override Task OnInitializedAsync()
    {
        CurrentMessages = new();
        InstructorConversations = new();
        StudentConversations = new();
        RecruiterConversations = new();
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
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

    #region [ Methods - Event Handlers ]
    public async void OnLoadOlderMessage()
    {

    }

    public async void OnChat()
    {

    }

    public async void OnChangeConversation()
    {

    }

    public async void OnRefresh()
    {

    }
    #endregion

    #region [ Methods - Data ]
    private async Task<User> GetCurrentUserAsync()
    {
        var user = await LocalStorage.GetItemAsync<User>("login-user-info");
        if(user == null)
        {
            NavigationManager.NavigateTo("/", true);
            return null;
        }

        return user;
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var user = await GetCurrentUserAsync();
            Sender = user;

            if (IsAdminViewing)
            {
                var instructorRole = await Roles.FindByNameAsync("instructor");

                var adminConversations = await Conversations.FindAll(x => x.Users.Where(x => x.Id == user.Id).Any()).Include(x => x.Users).AsNoTracking().ToListAsync();
                var admin_insConversations = adminConversations
                                                    .Where(x => x.Users
                                                        .Where(x => x.UserRoles
                                                            .Where(x => x.RoleId == instructorRole.Id).Any())
                                                        .Any()).ToList();
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
                                                        .Any()).ToList();
                StudentConversations = ins_studentConversations;

                var ins_recruiterConversations = instructorConversations
                                                    .Where(x => x.Users
                                                        .Where(x => x.UserRoles
                                                            .Where(x => x.RoleId == recruiterRole.Id).Any())
                                                        .Any()).ToList();
                RecruiterConversations = ins_recruiterConversations;

                //load student's avatar
                var studentIds = new List<string>();
                foreach(var x in StudentConversations.Select(x => x.Users))
                {
                    studentIds.AddRange(x.Select(x => x.Id));
                }

                var students = await Students.FindAll(x => studentIds.Contains(x.Id)).Select(x => new Student()
                                                                                                    {
                                                                                                        Id = x.Id,
                                                                                                        ImgUrl = x.ImgUrl,
                                                                                                    }).ToListAsync();
                if(students != null && students.Count > 0)
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
                    var instructorId = student.InternGroup.InstructorId;

                    var studentConversations = await Conversations.FindAll(x => x.Users.Where(x => x.Id == user.Id).Any()).Include(x => x.Users).AsNoTracking().ToListAsync();
                    var student_insConversation = studentConversations.Where(x => x.Users.Where(x => x.Id == instructorId).Any()).FirstOrDefault();
                    InstructorConversation = student_insConversation;
                }
            }
            else if (IsRecruiterViewing)
            {
                var instructor = await Instructors.FindAll(x => x.IsDepartmentManager).AsNoTracking().FirstOrDefaultAsync();
                if (instructor != null)
                {
                    var recruiterConversations = await Conversations.FindAll(x => x.Users.Where(x => x.Id == user.Id).Any()).Include(x => x.Users).AsNoTracking().ToListAsync();
                    var recruiter_insConversation = recruiterConversations.Where(x => x.Users.Where(x => x.Id == instructor.Id).Any()).FirstOrDefault();
                    InstructorConversation = recruiter_insConversation;
                }
            }

            OnInitializeContext();
        }
        catch (Exception)
        {
            
        }
        finally
        {
            StateHasChanged();
        }
    }

    private void OnInitializeContext()
    {
        var conversationContext = new ConversationContext()
        {
            Sender = Sender,
        };

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
            conversationContext.AdminConversation = AdminConversation.ToListRow(receiverName: "Administrator");

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
            conversationContext.InstructorConversation = InstructorConversation.ToListRow(receiverName: InstructorConversation.Users.FirstOrDefault(x => x.Id != Sender.Id)?.FullName);
        }
        else if (IsRecruiterViewing)
        {
            conversationContext.InstructorConversation = InstructorConversation.ToListRow(receiverName: InstructorConversation.Users.FirstOrDefault(x => x.Id != Sender.Id)?.FullName);
        }

        ConversationContext = conversationContext;
        ChatContext = new();
    }
    #endregion
}
