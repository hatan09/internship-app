using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using RCode.UI.ViewModels;

namespace InternshipApp.Portal.Views;

public partial class SkillScoreFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public ISkillScoreRepository SkillScores { get; private set; }

    [Inject]
    public ISkillRepository Skills { get; private set; }
    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, SkillScore> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<SkillScore>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected SkillScoreFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new ();
        this.Context = new(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, SkillScore>>(nameof(this.FormRequest));
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
            if(FormRequest.Data.SkillId < 1)
            {
                return;
            }

            this.States = this.FormRequest.Data.ToFormViewStates();
            var existingSkillScores = await SkillScores.FindAll(x => x.SkillId == FormRequest.Data.SkillId).ToListAsync();
            var allSkills = await Skills
                .FindAll(x => !existingSkillScores.Select(x => x.AlternativeSkillId).Contains(x.Id))
                .AsNoTracking()
                .ToListAsync();
            var skill = allSkills.FirstOrDefault(x => x.Id == FormRequest.Data.SkillId);
            
            allSkills.Remove(skill);
            if (FormRequest.Action == FormAction.Add)
            {
                States.Skills = allSkills.ToObservableCollection();
            }
            else
            {
                var alternativeSkill = await Skills.FindByIdAsync(FormRequest.Data.AlternativeSkillId);
                States.Name = alternativeSkill.Name;
            }
            States.MasterSkillName = skill.Name;

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
            this.FormRequest.Data.AlternativeSkillId = int.Parse(States.GetSelectedSkillId());
            this.SkillScores.Add(this.FormRequest.Data);
            await SkillScores.SaveChangesAsync();

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
            var skillScore = await SkillScores.FindAll(x => x.Id == this.FormRequest.Data.Id).AsTracking().FirstOrDefaultAsync();
            if (skillScore == null)
            {
                return;
            }
            skillScore.Matching = Enum.Parse<MatchingType>(States.GetSelectedTypeName());
            
            this.SkillScores.Update(skillScore);
            await SkillScores.SaveChangesAsync();

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
        this.States = new SkillScore().ToFormViewStates();
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
