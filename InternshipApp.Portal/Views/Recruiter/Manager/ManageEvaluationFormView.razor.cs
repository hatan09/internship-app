using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Wave5.UI.Forms;
using Microsoft.EntityFrameworkCore;
using InternshipApp.Repository;

namespace InternshipApp.Portal.Views;

public partial class ManageEvaluationFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public IJobRepository Jobs { get; private set; }

    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public IEvaluationRepository Evaluations { get; set; }
    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, Evaluation> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<Evaluation>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected EvaluationFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new EvaluationFormViewStates();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, Evaluation>>(nameof(this.FormRequest));
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
            case FormAction.Add:
                await this.AddedAsync();
                break;
            case FormAction.Edit:
                await this.UpdatedAsync();
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
            var student = await Students.FindByIdAsync(FormRequest.Data.StudentId);
            var job = await Jobs.FindByIdAsync((int) FormRequest.Data.JobId);
            States.StudentName = student?.FullName;
            States.JobName = job?.Title;
            
            switch (this.FormRequest.Action)
            {
                
                case FormAction.Add:
                    {
                        
                        break;
                    }
                case FormAction.Edit:
                    {

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
            this.Evaluations.Add(States.ToEntity());
            await Evaluations.SaveChangesAsync();
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
            this.Evaluations.Update(States.ToEntity());
            await Evaluations.SaveChangesAsync();
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
        this.States = new Evaluation().ToFormViewStates();
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
