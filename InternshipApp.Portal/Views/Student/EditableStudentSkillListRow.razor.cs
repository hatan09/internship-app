using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;
using Wave5.UI;
using InternshipApp.Core.Entities;
using InternshipApp.Contracts;
using InternshipApp.Repository;
using Microsoft.EntityFrameworkCore;
using RCode.UI.ViewModels;

namespace InternshipApp.Portal.Views;

public partial class EditableStudentSkillListRow
{
    #region [ CTor ]
    public EditableStudentSkillListRow()
    {

    }
    #endregion

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    #region [ Properties - Parameter ]
    [Parameter]
    public FormRequest<FormAction, StudentSkill> FormRequest { get; set; }

    [Parameter]
    public List<Skill> AllSkills { get; set; }

    [Parameter]
    public EventCallback<StudentSkillListRowViewStates> UpdateCallback { get; set; }

    [Parameter]
    public EventCallback<StudentSkillListRowViewStates> AddCallback { get; set; }

    [Parameter]
    public EventCallback<int> DeleteCallback { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public StudentSkillListRowViewStates States { get; set; }

    protected EditContext Context { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, StudentSkill>>(nameof(this.FormRequest));
        var currentFormRequest = this.FormRequest;

        await base.SetParametersAsync(parameters);
        if (newFormRequest.Action != FormAction.Add)
        {
            if (newFormRequest != null &&
            !newFormRequest.Data.StudentId.Equals(currentFormRequest?.Data.StudentId))
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
        States.SkillId = int.Parse(States.GetSelectedSkillId());
        States.Level = States.GetSelectedLevelName();
        await UpdateCallback.InvokeAsync(States);
    }

    public async Task OnDelete()
    {
        await DeleteCallback.InvokeAsync(States.SkillId);
    }

    public async Task OnAdd()
    {
        States.SkillId = int.Parse(States.GetSelectedSkillId());
        States.Level = States.GetSelectedLevelName();
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

            if (AllSkills != null)
            {
                States.Skills = AllSkills.ToObservableCollection();

            }
            else
            {
                var allSkills = await Skills.FindAll().AsNoTracking().ToListAsync();
                States.Skills = allSkills.ToObservableCollection();
            }

            States.SelectedSkill = AllSkills.FirstOrDefault(x => x.Id == States.SkillId)?.Name;
            States.SelectedLevel = States.Level.ToString();
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
}
