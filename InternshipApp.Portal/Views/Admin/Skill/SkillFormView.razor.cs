using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class SkillFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public ISkillRepository Skills { get; private set; }

    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, Skill> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<Skill>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected SkillFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new SkillFormViewStates();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, Skill>>(nameof(this.FormRequest));
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
            this.FormRequest.Data = this.States.ToEntity();
            this.Skills.Add(this.FormRequest.Data);
            await Skills.SaveChangesAsync();

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
            this.Skills.Update(FormRequest.Data);
            await Skills.SaveChangesAsync();

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
        this.States = new Skill().ToFormViewStates();
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
