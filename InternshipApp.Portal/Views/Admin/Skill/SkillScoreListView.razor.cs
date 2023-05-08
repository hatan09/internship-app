using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;
using Wave5.UI;
using Wave5.UI.Blazor;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class SkillScoreListView
{
    #region [ Properties - Parameters ]
    [Parameter]
    public string SkillId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public ISkillScoreRepository SkillScores { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, SkillScore> SkillScoreFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<SkillScoreListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected SkillScoreListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            States = new();

            SearchContext = new DataListSearchContext();
            ListContainerContext = new DetailsListContainerContext();
            ListContext = new DetailsListContext<SkillScoreListRowViewStates>();
            ListContext.SelectionMode = SelectionMode.Single;
            ListContext.OnItemInvoked += OnRowClicked;
            ListContext.OnSelectionChanged += OnSelectionChanged;

            InitializeCommandBar();
            InitializeColumn();

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
            await LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Event Handlers - Search ]
    private async void OnSearchDatalist(ChangeEventArgs args)
    {
        var filtered = new List<SkillScoreListRowViewStates>();

        var value = args?.Value.ToString();
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = States.Items;
        }
        else
        {
            // have to check the string is not null first
            // for some reasons, the data can be null or blank
            filtered = States.Items.Where(x =>
                (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(SkillScoreListRowViewStates rowItem)
    {
        NavigationManager.NavigateTo($"admin-skill-details/{rowItem.Id}");
    }

    private void OnSelectionChanged()
    {
        var value = ListContext.GetSelectedItems().Any();
        CommandBarContext.SetItemIsVisible(ButtonNames.EditButton, value);
        CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, value);
        StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
        var name = new DataGridColumnDefinition<SkillScoreListRowViewStates>("Alternative Skill Name", x => x.Name)
        {
            ColumnDataKey = nameof(SkillScoreListRowViewStates.Name),
            Width = "3fr"
        };

        var type = new DataGridColumnDefinition<SkillScoreListRowViewStates>("Matching Type", x => x.MatchingType)
        {
            ColumnDataKey = nameof(SkillScoreListRowViewStates.MatchingType),
            Width = "1fr"
        };

        ListContext.Columns.Definitions.Add(name);
        ListContext.Columns.Definitions.Add(type);
    }
    #endregion

    #region [ Private Methods - CommandBars ]
    private void InitializeCommandBar()
    {
        // Search
        SearchContext.OnDatalistSearch = OnSearchDatalist;

        // Items
        CommandBarContext = new CommandBarContext();
        CommandBarContext.Items.AddRefreshButton(OnRefreshButtonClicked);
        CommandBarContext.Items.AddAddButton(OnAddButtonClicked);
        CommandBarContext.Items.AddEditButton(OnEditButtonClicked, false);
        CommandBarContext.Items.AddDeleteButton(OnDeleteButtonClicked, false);
    }
    #endregion

    #region [ Event Handlers - CommandBars - Buttons ]
    private async void OnRefreshButtonClicked(EventArgs e)
    {
        SearchContext.SetDefafultSearchValue(string.Empty);
        await LoadDataAsync();
    }

    private void OnAddButtonClicked(EventArgs e)
    {
        var item = new SkillScore();
        SkillScoreFormRequest = FormRequestFactory.AddRequest(item);
        StateHasChanged();
    }

    private void OnEditButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = ListContext.GetSelectedItems().FirstOrDefault();
            if (selectedItem == null)
            {
                return;
            }

            SkillScoreFormRequest = FormRequestFactory.EditRequest(selectedItem.ToEntity());
            StateHasChanged();

        }
        catch (Exception ex)
        {

        }
    }

    private async void OnDeleteButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = ListContext.GetSelectedItems();
            if (selectedItem.Count == 0)
            {
                return;
            }

            foreach (var item in selectedItem)
            {
                SkillScores.Delete(item.ToEntity());

            }

            await SkillScores.SaveChangesAsync();
            var tasks = new List<Task>
        {
            LoadDataAsync()
        };

            await Task.WhenAll(tasks);

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<SkillScore> result)
    {
        switch (result.State)
        {
            case FormResultState.Added:
            case FormResultState.Updated:
            case FormResultState.Deleted:
                var tasks = new List<Task>
            {
                LoadDataAsync()
            };

                await Task.WhenAll(tasks);
                break;
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            ListContainerContext.SetProcessingStates(true, false);
            SearchContext.SetProcessingStates(true);
            CommandBarContext.SetProcessingStates(true);
            ListContext.SetProcessingStates(true);
            States.Items.Clear();

            StateHasChanged();

            var skill = await Skills.FindByIdAsync(int.Parse(SkillId));
            var items = await SkillScores.FindAll(x => x.SkillId == skill.Id).AsNoTracking().ToListAsync();

            var skills = await Skills.FindAll(x => items.Select(x => x.AlternativeSkillId).Contains(x.Id)).ToListAsync();

            States.Items = items.ToListRowList();

            States.Items.ForEach(x =>
            {
                x.MasterSkillName = skill.Name;
                x.Name = skills.FirstOrDefault(y => y.Id == x.SkillId)?.Name;
            });
            ListContext.GetKey = (x => x.Id);
            ListContext.ItemsSource.Clear();
            ListContext.ItemsSource.AddRange(States.Items);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ListContainerContext.SetProcessingStates(false, ListContext.ItemsSource.Any());
            ListContext.SetProcessingStates(false);
            SearchContext.SetProcessingStates(false);
            CommandBarContext.SetProcessingStates(false);
            StateHasChanged();
        }
    }
    #endregion
}
