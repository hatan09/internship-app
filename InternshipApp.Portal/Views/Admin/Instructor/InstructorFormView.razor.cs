﻿using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Wave5.UI.Forms;
using InternshipApp.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class InstructorFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public InstructorManager Instructors { get; private set; }

    [Inject]
    public RoleManager<Role> Roles { get; private set; }

    [Inject]
    public IConversationRepository Conversations { get; private set; }

    [Inject]
    public UserManager Users { get; private set; }
    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, Instructor> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<Instructor>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected InstructorFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new InstructorFormViewStates();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, Instructor>>(nameof(this.FormRequest));
        var currentFormRequest = this.FormRequest;

        await base.SetParametersAsync(parameters);

        if (newFormRequest != null && newFormRequest != currentFormRequest)
        {
            if (newFormRequest.Action == FormAction.Cancel)
            {
                await this.CanceledAsync();
            }
            else
            {
                await this.LoadDataAsync();
            }
            return;
        }
    }
    #endregion

    #region [ Event Handlers - Form ]
    private void OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        
        this.StateHasChanged();
    }

    private async void OnFormSubmit(object args)
    {
        switch (this.FormRequest.Action)
        {
            case FormAction.Edit:
                await this.UpdatedAsync();
                break;
            case FormAction.Add:
                await this.AddedAsync();
                break;
        }
    }

    private async void OnCancel(object args)
    {
        await this.CanceledAsync();
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            this.States = this.FormRequest.Data.ToFormViewStates();
            States.IsManager = FormRequest.Data.IsDepartmentManager;

            switch (this.FormRequest.Action)
            {
                case FormAction.Add:
                    {
                        // Add action logic
                        break;
                    }
                case FormAction.Edit:
                    {
                        // Edit action logic
                        break;
                    }
                case FormAction.Delete:
                case FormAction.Details:
                default:
                    break;
            }

            this.Context = new EditContext(this.States);
            this.Context.OnFieldChanged += this.OnFieldChanged;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.StateHasChanged();
        }
    }
    #endregion

    #region [ Private Methods - CRUD ]
    private async Task AddedAsync()
    {
        try
        {
            var instructor = this.States.ToEntity();

            if (States.IsDepartmentManager)
            {
                var oldManager = await Instructors.FindAll(x => x.IsDepartmentManager).AsTracking().FirstOrDefaultAsync();
                if (oldManager != null)
                {
                    oldManager.IsDepartmentManager = false;
                    await Instructors.UpdateAsync(oldManager);
                }
            }
            instructor.IsDepartmentManager = States.IsDepartmentManager;

            var result = await Instructors.CreateAsync(instructor);
            if (!result.Succeeded)
            {
                return;
            }

            result = await Instructors.AddPasswordAsync(instructor, States.Password);
            if (!result.Succeeded)
            {
                return;
            }

            result = await Instructors.AddToRoleAsync(instructor, "instructor");
            if (!result.Succeeded)
            {
                return;
            }

            var adminRole = await Roles.FindByNameAsync("admin");
            var admin = await Users.FindAll(x => x.UserRoles.Where(x => x.RoleId == adminRole.Id).Any()).AsTracking().FirstOrDefaultAsync();

            var conversation = new Conversation()
            {
                LastMessageTime = DateTime.Now,
                Title = $"{instructor.FullName} - ADMIN",
            };
            conversation.Users.Add(admin);
            conversation.Users.Add(instructor);

            Conversations.Add(conversation);
            await Conversations.SaveChangesAsync();

            await this.InvokeFormResultCallbackAsync(FormResultState.Added);
        }
        catch (Exception ex)
        {

        }
        finally
        {
        }
    }

    private async Task UpdatedAsync()
    {
        try
        {
            var recruiter = await Instructors.FindByIdAsync(States.Id);
            if(recruiter == null)
            {
                return;
            }

            if(!States.IsManager && States.IsDepartmentManager)
            {
                var oldManager = await Instructors.FindAll(x => x.IsDepartmentManager).AsTracking().FirstOrDefaultAsync();
                if(oldManager!= null)
                {
                    oldManager.IsDepartmentManager = false;
                    await Instructors.UpdateAsync(oldManager);
                }
            }

            recruiter.FullName = States.Name;
            recruiter.Email = States.Email;
            recruiter.IsDepartmentManager = States.IsDepartmentManager;

            await Instructors.UpdateAsync(recruiter);

            await this.InvokeFormResultCallbackAsync(FormResultState.Updated);
        }
        catch (Exception ex)
        {

        }
        finally
        {
        }
    }

    private async Task CanceledAsync()
    {
        await this.InvokeFormResultCallbackAsync(FormResultState.Canceled);
    }

    private void Reset()
    {
        this.States = new Instructor().ToFormViewStates();
        this.Context = new EditContext(this.States);

        this.StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Callbacks ]
    private async Task InvokeFormResultCallbackAsync(FormResultState state)
    {
        var result = FormResultFactory.New(state, this.FormRequest.Data);
        await this.FormResultCallback.InvokeAsync(result);

        this.Reset();
    }
    #endregion
}
