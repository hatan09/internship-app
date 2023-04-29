using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;
using Wave5.UI;
using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public partial class EditableStudentSkillListRow
{
    #region [ CTor ]
    public EditableStudentSkillListRow()
    {

    }
    #endregion

    [Inject]
    public ILogger<EditableStudentSkillListRow> Logger { get; set; }

    #region [ Properties - Parameter ]
    [Parameter]
    public FormRequest<FormAction, StudentSkill> FormRequest { get; set; }

    [Parameter]
    public string CorrectAnswerId { get; set; }

    //[Parameter]
    //public List<DataGridColumnDefinition<StudentSkillListRowViewStates>> Columns { get; set; }

    [Parameter]
    public EventCallback<StudentSkillListRowViewStates> UpdateCallback { get; set; }

    [Parameter]
    public EventCallback<StudentSkillListRowViewStates> AddCallback { get; set; }

    [Parameter]
    public EventCallback<int> ModifyCorrectAnswerCallback { get; set; }

    [Parameter]
    public EventCallback<int> DeleteCallback { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public StudentSkillListRowViewStates States { get; set; }

    protected EditContext Context { get; private set; }

    protected bool IsCorrectAnswer { get; private set; } = false;
    #endregion

    #region [ Properties - Elements ]
    protected MessageBarContainer Messages { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, StudentSkill>>(nameof(this.FormRequest));
        var currentFormRequest = this.FormRequest;

        //refresh when update correct answer
        var currentCorrectAnswerId = this.CorrectAnswerId;
        var newCorrectAnswerId = parameters.GetValueOrDefault<string>(nameof(this.CorrectAnswerId));

        await base.SetParametersAsync(parameters);
        if (newFormRequest.Action != FormAction.Add)
        {
            if (newFormRequest != null &&
            !newFormRequest.Data.StudentId.Equals(currentFormRequest?.Data.StudentId))
            {
                await this.LoadDataAsync();
                return;
            }
            else if (newCorrectAnswerId is not null && !newCorrectAnswerId.Equals(currentCorrectAnswerId))
            {
                await this.LoadDataAsync();
                return;
            }
        }

    }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            States = new StudentSkillListRowViewStates();
            Context = new EditContext(States);

            await this.LoadDataAsync();
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex.ToString());
            // TODO: this.MessageProvider.AddExceptionMessage(this, ex);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task Reset()
    {
        await this.LoadDataAsync();
    }
    #endregion

    #region [ Event Handlers - Answers ]
    public async Task OnUpdate()
    {
        await UpdateCallback.InvokeAsync(States);
    }

    public async Task OnModifyCorrectAnswer()
    {
        if (!IsCorrectAnswer)
        {
            await ModifyCorrectAnswerCallback.InvokeAsync(string.Empty);
        }
        else
        {
            await ModifyCorrectAnswerCallback.InvokeAsync(States.Id);
        }
    }

    public async Task OnDelete()
    {
        await DeleteCallback.InvokeAsync(States.Id);
    }

    public async Task OnAdd()
    {
        await AddCallback.InvokeAsync(States);
        await Reset();
    }
    #endregion

    #region [ Event Handlers - Form ]
    private async void OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        this.StateHasChanged();
        if (FormRequest?.Action == FormAction.Edit)
        {
            await OnUpdate();
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            this.States = this.FormRequest.Data.ToListRow();

            this.Context = new EditContext(this.States);
            this.IsCorrectAnswer = this.States.Id.Equals(this.CorrectAnswerId);
            this.Context.OnFieldChanged += this.OnFieldChanged;
        }
        catch (Exception ex)
        {
            this.Messages.AddErrorMessage(ex.Message, false);
        }
        finally
        {
            this.StateHasChanged();
        }
    }
    #endregion
}
