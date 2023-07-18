using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Wave5.UI.Forms;
using InternshipApp.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class RecruiterFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public RecruiterManager Recruiters { get; private set; }

    [Inject]
    public RoleManager<Role> Roles { get; private set; }

    [Inject]
    public InstructorManager Instructors { get; private set; }

    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, Recruiter> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<Recruiter>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected RecruiterFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new RecruiterFormViewStates();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, Recruiter>>(nameof(this.FormRequest));
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
            var recruiter = this.States.ToEntity();

            var result = await Recruiters.CreateAsync(recruiter);
            if (!result.Succeeded)
            {
                return;
            }

            result = await Recruiters.AddPasswordAsync(recruiter, States.Password);
            if (!result.Succeeded)
            {
                return;
            }

            result = await Recruiters.AddToRoleAsync(recruiter, "recruiter");
            if (!result.Succeeded)
            {
                return;
            }

            var instructor = await Instructors.FindAll(x => x.IsDepartmentManager).AsTracking().FirstOrDefaultAsync();
            var createdRecruiter = await Recruiters.FindAll(x => x.UserName == recruiter.UserName).AsTracking().FirstOrDefaultAsync();
            if(instructor != null && createdRecruiter != null)
            {
                var newConversation = new Conversation()
                {
                    LastMessageTime= DateTime.Now,
                    Title = $"{instructor.FullName}_{createdRecruiter.FullName}",
                    Users = {instructor, createdRecruiter}
                };
            }

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
            var recruiter = await Recruiters.FindByIdAsync(States.Id);
            if(recruiter == null)
            {
                return;
            }

            recruiter.FullName = States.Name;
            recruiter.Email = States.Email;

            await Recruiters.UpdateAsync(recruiter);

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
        this.States = new Recruiter().ToFormViewStates();
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
