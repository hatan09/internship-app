using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Wave5.UI.Forms;
using InternshipApp.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class InternGroupFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public InstructorManager Instructors { get; set; }

    [Inject]
    public IInternGroupRepository InternGroups { get; private set; }

    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, InternGroup> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<InternGroup>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected InternGroupFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new InternGroupFormViewStates();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, InternGroup>>(nameof(this.FormRequest));
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
        if (e.FieldIdentifier.FieldName.Equals(nameof(States.InstructorName)))
        {
            if (string.IsNullOrWhiteSpace(States.Title) && !string.IsNullOrWhiteSpace(States.InstructorName))
            {
                States.Title = States.InstructorName + "_Group";
            }
        }
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
                        var allInstructors = await Instructors.FindAll(x => x.InternGroup == null).AsNoTracking().ToListAsync();
                        States.Instructors.AddRange(allInstructors!);
                        break;
                    }
                case FormAction.Edit:
                    {
                        var instructor = await Instructors.FindByIdAsync(States.InstructorId);
                        if(instructor != null)
                        {
                            States.InstructorName = instructor.FullName;
                        }
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
            var insId = States.GetSelectedInstructorId();
            this.FormRequest.Data.InstructorId = insId;
            this.InternGroups.Add(this.FormRequest.Data);
            await InternGroups.SaveChangesAsync();

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
            this.InternGroups.Update(FormRequest.Data);
            await InternGroups.SaveChangesAsync();

            await this.InvokeFormResultCallbackAsync(FormResultState.Added);
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
        this.States = new InternGroup().ToFormViewStates();
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
