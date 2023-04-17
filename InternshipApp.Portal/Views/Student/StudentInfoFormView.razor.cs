using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;
using Wave5.UI;
using InternshipApp.Repository;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternshipApp.Portal.Views;

public partial class StudentInfoFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public RoleManager<Role> Roles { get; private set; }

    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, Student> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<Student>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected StudentFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new StudentFormViewStates();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, Student>>(nameof(this.FormRequest));
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
        if (e.FieldIdentifier.FieldName.Equals(nameof(States.StudentId)))
        {
            if(string.IsNullOrWhiteSpace(States.Username) && !string.IsNullOrWhiteSpace(States.StudentId))
            {
                States.Username = States.StudentId;
            }
        }
        this.StateHasChanged();
    }

    private async void OnFormSubmit(object args)
    {
        switch (this.FormRequest.Action)
        {
            case FormAction.Add:
                await this.AddedAsync();
                break;

            case FormAction.Edit:
                if (!this.Context.IsModified())
                {
                    await this.CanceledAsync();
                }
                else
                {
                    await this.UpdatedAsync();
                }
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
            this.FormRequest.Data = this.States.ToEntity();
            await this.Students.CreateAsync(this.FormRequest.Data);

            var role = await Roles.FindByNameAsync("student");

            if (role != null)
            {
                await Students.AddToRoleAsync(FormRequest.Data, role.Name);
            }

            await Students.AddPasswordAsync(FormRequest.Data, States.Password?? $"password{FormRequest.Data.StudentId}");

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
            this.FormRequest.Data = this.States.ToEntity();

            await this.Students.UpdateAsync(this.FormRequest.Data);

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
        this.States = new Student().ToFormViewStates();
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
