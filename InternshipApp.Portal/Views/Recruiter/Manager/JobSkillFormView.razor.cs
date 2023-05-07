using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Wave5.UI.Forms;
using InternshipApp.Contracts;
using Microsoft.EntityFrameworkCore;
using RCode.UI.ViewModels;

namespace InternshipApp.Portal.Views;

public partial class JobSkillFormView
{
    #region [ Properties - Inject ]
    [Inject]
    public IJobRepository Jobs { get; private set; }

    [Inject]
    public ISkillRepository Skills { get; private set; }

    [Inject]
    public RoleManager<Role> Roles { get; private set; }

    #endregion

    #region [ Properties - Parameters ]
    [EditorRequired]
    [Parameter]
    public FormRequest<FormAction, JobSkill> FormRequest { get; set; }

    [EditorRequired]
    [Parameter]
    public EventCallback<FormResult<JobSkill>> FormResultCallback { get; set; }
    #endregion

    #region [ Properties - Data ]
    protected EditContext Context { get; private set; }

    protected JobSkillFormViewStates States { get; private set; }
    #endregion

    #region [ Event Handlers - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.States = new JobSkillFormViewStates();
        this.Context = new EditContext(this.States);

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newFormRequest = parameters.GetValueOrDefault<FormRequest<FormAction, JobSkill>>(nameof(this.FormRequest));
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

            switch (this.FormRequest.Action)
            {
                case FormAction.Add:
                    {
                        var skills = await Skills.FindAll().ToListAsync();
                        States.Skills = skills.ToObservableCollection();
                        break;
                    }
                case FormAction.Edit:
                    {
                        var skill = await Skills.FindByIdAsync(FormRequest.Data.SkillId);
                        States.SelectedLevel = FormRequest.Data.Level.ToString();
                        States.SelectedSkill = skill.Name;
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
            if(double.TryParse(States.WeightText, out var weight))
            {
                FormRequest.Data.Weight = weight;
            }
            else
            {
                return;
            }

            var job = await Jobs.FindAll(x => x.Id == FormRequest.Data.JobId).AsTracking()
                .Include(x => x.JobSkills)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return;
            }

            if (    (FormRequest.Data.Weight > 1.0 && FormRequest.Data.Weight < 0.0) ||
                    job.JobSkills.Sum(x => x.Weight) == 1.0 ||
                    job.JobSkills.Sum(x => x.Weight) + FormRequest.Data.Weight > 1.0)
            {
                FormRequest.Data.Weight = 0.0;
            }

            FormRequest.Data.JobId = job.Id;
            FormRequest.Data.SkillId = int.Parse(States.GetSelectedSkillId());
            FormRequest.Data.Level = Enum.Parse<SkillLevel>(States.SelectedLevel);
            job.JobSkills.Add(FormRequest.Data);
            Jobs.Update(job);
            await Jobs.SaveChangesAsync();
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
            if (double.TryParse(States.WeightText, out var weight))
            {
                FormRequest.Data.Weight = weight;
            }
            else
            {
                return;
            }

            var job = await Jobs.FindAll(x => x.Id == FormRequest.Data.JobId)
                .AsTracking()
                .Include(x => x.JobSkills)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return;
            }

            if ((FormRequest.Data.Weight > 1.0 && FormRequest.Data.Weight < 0.0) ||
                    job.JobSkills.Sum(x => x.Weight) == 1.0 ||
                    job.JobSkills.Sum(x => x.Weight) + FormRequest.Data.Weight > 1.0)
            {
                FormRequest.Data.Weight = 0.0;
            }

            FormRequest.Data.JobId = job.Id;
            FormRequest.Data.Level = Enum.Parse<SkillLevel>(States.SelectedLevel);
            job.JobSkills.Remove(job.JobSkills.Where(x => x.SkillId == FormRequest.Data.SkillId).First());
            job.JobSkills.Add(FormRequest.Data);
            await Jobs.SaveChangesAsync();
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
        this.States = new JobSkill().ToFormViewStates();
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
