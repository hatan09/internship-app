using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using RCode;
using Wave5.UI.Forms;
using Wave5.UI;
using Wave5.UI.Blazor;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class SkillDetailsView
{
	#region [ Properties - Parameters ]
	[Parameter]
	public string SkillId { get; set; }
    #endregion

    #region [ Properties ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public StudentManager Students { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Skill> SkillFormRequest { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsCardContainerContext DetailsContainerContext { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected SkillDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        this.CommandBarContext = new CommandBarContext();
        this.DetailsContainerContext = new DetailsCardContainerContext();
        States = new();
        this.InitializeCommandBars();

        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentSkillId = this.SkillId;
        var parameterSkillId = parameters.GetValueOrDefault<string>(nameof(this.SkillId));

        await base.SetParametersAsync(parameters);

        if (currentSkillId != parameterSkillId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Private Methods - CommandBars ]
    private void InitializeCommandBars()
    {
        this.CommandBarContext.Items.AddEditButton(OnEditButtonClicked, true);
    }
    #endregion

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<Skill> result)
    {
        switch (result.State)
        {
            case FormResultState.Added:
            case FormResultState.Updated:
            case FormResultState.Deleted:
                await this.LoadDataAsync();
                break;
        }
    }

    protected void OnEditButtonClicked(EventArgs args)
    {
        SkillFormRequest = FormRequestFactory.EditRequest(States.ToEntity());
        StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.SkillId, nameof(this.SkillId));

        try
        {
            this.DetailsContainerContext.SetProcessingStates(true, false);
            this.CommandBarContext.SetProcessingStates(true);

            var item = await this.Skills
                .FindAll(x => x.Id == int.Parse(SkillId)).AsNoTracking()
                .FirstOrDefaultAsync();

            if (item is null)
            {
                this.States = null;
                return;
            }


            this.States = item.ToDetailsViewStates();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.DetailsContainerContext.SetProcessingStates(false, this.States != null);
            this.CommandBarContext.SetProcessingStates(false, this.States != null);
            this.StateHasChanged();
        }
    }
    #endregion
}
