using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;
using Wave5.UI;
using Wave5.UI.Blazor;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class JobSkillListView
{
    [Parameter]
    public string JobId { get; set; }

    #region [ Properties - Inject ]
    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public ICompanyRepository Companies { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, JobSkill> JobSkillFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<JobSkillListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected JobSkillListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            States = new();

            SearchContext = new DataListSearchContext();
            ListContainerContext = new DetailsListContainerContext();
            ListContext = new DetailsListContext<JobSkillListRowViewStates>();
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
        var filtered = new List<JobSkillListRowViewStates>();

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
                (!string.IsNullOrEmpty(x.SkillName) && x.SkillName.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(JobSkillListRowViewStates rowItem)
    {
        NavigationManager.NavigateTo($"/manage-job-info/{rowItem.Id}");
    }

    private void OnSelectionChanged()
    {
        var value = ListContext.GetSelectedItems().Any();
        CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, value);
        StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
        var title = new DataGridColumnDefinition<JobSkillListRowViewStates>("Skill Title", x => x.SkillName)
        {
            ColumnDataKey = nameof(JobSkillListRowViewStates.SkillName),
            Width = "2fr"
        };

        var level = new DataGridColumnDefinition<JobSkillListRowViewStates>("Level", x => x.Level)
        {
            ColumnDataKey = nameof(JobSkillListRowViewStates.Level),
            Width = "1fr"
        };

        var weight = new DataGridColumnDefinition<JobSkillListRowViewStates>("Weight", x => x.Weight)
        {
            ColumnDataKey = nameof(JobSkillListRowViewStates.Weight),
            Width = "1fr"
        };

        var description = new DataGridColumnDefinition<JobSkillListRowViewStates>("Description", x => x.Description)
        {
            ColumnDataKey = nameof(JobSkillListRowViewStates.Description),
            Width = "3fr"
        };

        ListContext.Columns.Definitions.Add(title);
        ListContext.Columns.Definitions.Add(level);
        ListContext.Columns.Definitions.Add(weight);
        ListContext.Columns.Definitions.Add(description);
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
        var item = new JobSkill();
        JobSkillFormRequest = FormRequestFactory.AddRequest(item);
        StateHasChanged();
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
                var job = await Jobs.FindAll(x => x.Id == item.JobId).Include(x => x.JobSkills).FirstOrDefaultAsync();
                if(job == null)
                {
                    continue;
                }
                job.JobSkills.Remove(job.JobSkills.FirstOrDefault(x => x.SkillId == item.SkillId));
            }

            await Jobs.SaveChangesAsync();
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
    protected async Task OnFormResultReceived(FormResult<JobSkill> result)
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
    private async Task<int> GetCompanyId()
    {
        var company = await Companies.FindAll().FirstOrDefaultAsync();
        return company == null ? 1 : company.Id;
    }

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

            var items = new List<JobSkill>();
            if(!string.IsNullOrEmpty(JobId)) {
                var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).Include(x => x.JobSkills).FirstOrDefaultAsync();
                if(job != null)
                {
                    items = job.JobSkills.ToList();
                }
            }

            States.Items = items.ToListRowList();
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
