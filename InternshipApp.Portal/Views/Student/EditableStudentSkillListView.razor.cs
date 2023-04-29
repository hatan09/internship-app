using Microsoft.AspNetCore.Components;
using RCode;
using Wave5.UI.DataGrids;
using Wave5.UI;
using InternshipApp.Core.Entities;
using Wave5.UI.Blazor;

namespace InternshipApp.Portal.Views;

public partial class EditableStudentSkillListView
{
    #region [ CTor ]
    public EditableStudentSkillListView()
    {

    }
    #endregion

    #region [ Properties - Inject ]
    #endregion

    #region [ Parameters ]
    [Parameter]
    public string QuestionId { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DetailsListContext<StudentSkillListRowViewStates> ListContext { get; private set; }

    protected List<DataGridColumnDefinition<StudentSkillListRowViewStates>> Columns { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected StudentSkillListViewStates States { get; private set; }

    protected List<StudentSkill> Items { get; private set; }

    protected string CorrectAnswerId { get; private set; }

    protected bool IsAddRowShowed { get; private set; }

    protected bool IsAddDisable { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            this.States = new StudentSkillListViewStates()
            {
                Items = new List<StudentSkillListRowViewStates>()
            };
            IsAddRowShowed = false;
            Items = new List<StudentSkill>();
            this.ListContainerContext = new DetailsListContainerContext();
            this.ListContext = new DetailsListContext<StudentSkillListRowViewStates>();

            this.InitializeColumn();

            Columns = ListContext.Columns?.Definitions;

            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await this.LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnParametersSetAsync()
    {
        IsAddRowShowed = false;
        if (!string.IsNullOrEmpty(QuestionId))
        {
            await LoadDataAsync();
        }

        await base.OnParametersSetAsync();
    }
    #endregion

    #region [ Event Handlers - DataList ]
    public async Task OnUpdate(StudentSkillListRowViewStates viewStates)
    {
        Guard.ParamIsNull(viewStates, nameof(viewStates));
        var entity = viewStates.ToEntity();
        await UpdateAsync(entity);
    }

    public async Task OnModifyCorrectAnswer(string answerId)
    {
        await this.LoadDataAsync();
    }

    public async Task OnAdd(StudentSkillListRowViewStates viewStates)
    {
        Guard.ParamIsNull(viewStates, nameof(viewStates));
        var entity = viewStates.ToEntity();

        await AddAsync(entity);
        await this.LoadDataAsync();
        OnToggleAdd(null);
    }

    public async Task OnDelete(string answerId)
    {
        Guard.ParamIsNullOrEmpty(answerId, nameof(answerId));
        await DeleteAsync(answerId);
        await this.LoadDataAsync();
    }
    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            this.ListContainerContext.SetProcessingStates(true, false);
            this.ListContext.SetProcessingStates(true);
            this.States.Items.Clear();

            this.StateHasChanged();

            if (!string.IsNullOrEmpty(QuestionId))
            {
                
                
            }

            this.ListContext.GetKey = (x => x.Id);
            // TODO: await this.AppLogicProvider.InvokeLoadDelayAsync();
        }
        catch (Exception ex)
        {
        }
        finally
        {
            this.ListContainerContext.SetProcessingStates(false, this.ListContext.ItemsSource.Any());
            this.ListContext.SetProcessingStates(false);
            this.StateHasChanged();
        }
    }

    private async Task UpdateAsync(StudentSkill StudentSkill)
    {
        //await this.LogicContext.StudentSkills.UpdateAsync(StudentSkill);
    }

    private async Task AddAsync(StudentSkill StudentSkill)
    {
        //await this.LogicContext.StudentSkills.AddAsync(StudentSkill);
    }

    private async Task DeleteAsync(string answerId)
    {
        Console.WriteLine($"{answerId} is deleted!");
        //await this.LogicContext.StudentSkills.DeleteAsync(answerId);
    }
    #endregion

    #region [ Private Methods - Row ]
    private void OnToggleAdd(object args)
    {
        IsAddRowShowed = !IsAddRowShowed;
        this.StateHasChanged();
    }
    #endregion
}
