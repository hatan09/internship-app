using Microsoft.AspNetCore.Components;
using RCode;
using Wave5.UI.DataGrids;
using Wave5.UI;
using InternshipApp.Core.Entities;
using Wave5.UI.Blazor;
using InternshipApp.Repository;
using InternshipApp.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class EditableStudentSkillListView
{
    #region [ CTor ]
    public EditableStudentSkillListView()
    {

    }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }
    #endregion

    #region [ Parameters ]
    [Parameter]
    public string StudentId { get; set; }

    [Parameter]
    public List<Skill> AllSkills { get; set; }

    [Parameter]
    public EventCallback EditSkillCallback { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DetailsListContext<StudentSkillListRowViewStates> ListContext { get; private set; }

    protected List<DataGridColumnDefinition<StudentSkillListRowViewStates>> Columns { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected StudentSkillListViewStates States { get; private set; }

    protected List<StudentSkill> Items { get; private set; }

    protected bool IsAddRowShown { get; private set; }

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
            IsAddRowShown = false;
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
            //await this.LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnParametersSetAsync()
    {
        IsAddRowShown = false;
        if (!string.IsNullOrEmpty(StudentId))
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
        if(EditSkillCallback.HasDelegate) await EditSkillCallback.InvokeAsync();
    }

    public async Task OnAdd(StudentSkillListRowViewStates viewStates)
    {
        Guard.ParamIsNull(viewStates, nameof(viewStates));
        var entity = viewStates.ToEntity();

        await AddAsync(entity);
        if (EditSkillCallback.HasDelegate) await EditSkillCallback.InvokeAsync();
        await this.LoadDataAsync();
        IsAddRowShown = false;
    }

    public async Task OnDelete(int answerId)
    {
        Guard.ParamIsNull(answerId, nameof(answerId));
        await DeleteAsync(answerId);
        if (EditSkillCallback.HasDelegate) await EditSkillCallback.InvokeAsync();
        await this.LoadDataAsync();
    }
    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
        var name = new DataGridColumnDefinition<StudentSkillListRowViewStates>("Description", x => x.Description)
        {
            ColumnDataKey = nameof(StudentSkillListRowViewStates.Description),
            Width = "3fr"
        };

        this.ListContext.Columns.Definitions.Add(name);

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

            if(AllSkills == null || AllSkills.Count == 0)
            {
                AllSkills = await Skills.FindAll().AsNoTracking().ToListAsync();
            }

            if (!string.IsNullOrEmpty(StudentId))
            {
                var student = await Students.FindAll(x => x.Id == StudentId)
                    .Include(x => x.StudentSkills)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (student != null)
                {
                    this.Items.Clear();
                    Items.AddRange(student.StudentSkills.ToList());
                }
            }

            States.Items.AddRange(Items.ToListRowList());
            this.ListContext.GetKey = x => x.Id;
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

    private async Task UpdateAsync(StudentSkill studentSkill)
    {
        var student = await this.Students.FindAll(x => x.Id == StudentId).AsTracking().Include(x => x.StudentSkills).FirstOrDefaultAsync();
        if(student != null)
        {
            student.StudentSkills.Remove(student.StudentSkills.FirstOrDefault(x => x.SkillId == studentSkill.SkillId));
            student.StudentSkills.Add(studentSkill);
            await Students.UpdateAsync(student);
        }

    }

    private async Task AddAsync(StudentSkill studentSkill)
    {
        var student = await this.Students.FindAll(x => x.Id == StudentId).AsTracking().Include(x => x.StudentSkills).FirstOrDefaultAsync();
        if (student != null)
        {
            if (student.StudentSkills.Contains(studentSkill))
                return;
            studentSkill.StudentId = StudentId;
            student.StudentSkills.Add(studentSkill);
            await Students.UpdateAsync(student);
        }
    }

    private async Task DeleteAsync(int skillId)
    {
        var student = await this.Students.FindAll(x => x.Id == StudentId).AsTracking().Include(x => x.StudentSkills).FirstOrDefaultAsync();
        if (student != null)
        {
            student.StudentSkills.Remove(student.StudentSkills.FirstOrDefault(x => x.SkillId == skillId));
            await Students.UpdateAsync(student);
            Items.Remove(Items.FirstOrDefault(x => x.SkillId == skillId));
            States.Items.Remove(States.Items.FirstOrDefault(x => x.SkillId == skillId));
            StateHasChanged();
        }
    }
    #endregion

    #region [ Private Methods - Row ]
    private void OnToggleAdd(object args)
    {
        IsAddRowShown = !IsAddRowShown;
        this.StateHasChanged();
    }
    #endregion
}
