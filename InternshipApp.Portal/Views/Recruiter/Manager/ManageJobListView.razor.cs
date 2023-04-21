using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class ManageJobListView : ComponentBase
{
    #region [ Properties - Inject ]
    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public ICompanyRepository Companies { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, Job> JobFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<JobListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected JobListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            this.States = new();

            this.SearchContext = new DataListSearchContext();
            this.ListContainerContext = new DetailsListContainerContext();
            this.ListContext = new DetailsListContext<JobListRowViewStates>();
            this.ListContext.SelectionMode = SelectionMode.Single;
            this.ListContext.OnItemInvoked += this.OnRowClicked;
            this.ListContext.OnSelectionChanged += this.OnSelectionChanged;

            this.InitializeCommandBar();
            this.InitializeColumn();

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
    #endregion

    #region [ Event Handlers - Search ]
    private async void OnSearchDatalist(ChangeEventArgs args)
    {
        var filtered = new List<JobListRowViewStates>();

        //await this.AppLogicProvider.InvokeSearchDelayAsync();

        var value = args?.Value.ToString();
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = this.States.Items;
        }
        else
        {
            // have to check the string is not null first
            // for some reasons, the data can be null or blank
            filtered = this.States.Items.Where(x =>
                (!string.IsNullOrEmpty(x.Title) && x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(JobListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/manage-job-info/{rowItem.Id}");
    }

    private void OnSelectionChanged()
    {
        var value = this.ListContext.GetSelectedItems().Any();
        this.CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, value);
        this.StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
        var title = new DataGridColumnDefinition<JobListRowViewStates>("Job Title", x => x.Title)
        {
            ColumnDataKey = nameof(JobListRowViewStates.Title),
            Width = "3fr"
        };

        var slots = new DataGridColumnDefinition<JobListRowViewStates>("Slots", x => x.Slots)
        {
            ColumnDataKey = nameof(JobListRowViewStates.Slots),
            Width = "1fr"
        };

        this.ListContext.Columns.Definitions.Add(title);
        this.ListContext.Columns.Definitions.Add(slots);
    }
    #endregion

    #region [ Private Methods - CommandBars ]
    private void InitializeCommandBar()
    {
        // Search
        this.SearchContext.OnDatalistSearch = this.OnSearchDatalist;

        // Items
        this.CommandBarContext = new CommandBarContext();
        this.CommandBarContext.Items.AddRefreshButton(this.OnRefreshButtonClicked);
        this.CommandBarContext.Items.AddAddButton(this.OnAddButtonClicked);
        this.CommandBarContext.Items.AddDeleteButton(this.OnDeleteButtonClicked, false);
    }
    #endregion

    #region [ Event Handlers - CommandBars - Buttons ]
    private async void OnRefreshButtonClicked(EventArgs e)
    {
        this.SearchContext.SetDefafultSearchValue(string.Empty);
        await this.LoadDataAsync();
    }

    private void OnAddButtonClicked(EventArgs e)
    {
        var item = new Job();
        JobFormRequest = FormRequestFactory.AddRequest(item);
        this.StateHasChanged();
    }

    private void OnEditButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = this.ListContext.GetSelectedItems();
            if (selectedItem.Count == 0)
            {
                return;
            }

            var item = selectedItem.FirstOrDefault();
            this.JobFormRequest = FormRequestFactory.EditRequest(item.ToEntity());

            this.StateHasChanged();
        }
        catch (Exception ex)
        {

        }
    }

    private async void OnDeleteButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = this.ListContext.GetSelectedItems();
            if (selectedItem.Count == 0)
            {
                return;
            }

            foreach (var item in selectedItem)
            {

            }
            var tasks = new List<Task>();

            tasks.Add(this.LoadDataAsync());

            await Task.WhenAll(tasks);

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<Job> result)
    {
        switch (result.State)
        {
            case FormResultState.Added:
            case FormResultState.Updated:
            case FormResultState.Deleted:
                var tasks = new List<Task>
                {
                    this.LoadDataAsync()
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
            this.ListContainerContext.SetProcessingStates(true, false);
            this.SearchContext.SetProcessingStates(true);
            this.CommandBarContext.SetProcessingStates(true);
            this.ListContext.SetProcessingStates(true);
            this.States.Items.Clear();

            this.StateHasChanged();

            var jobList = new List<Job>();
            var companyId = await GetCompanyId();
            var students = await Jobs.FindByCompanyId(companyId).ToListAsync();
            jobList.AddRange(students);

            this.States.Items.AddRange(jobList.ToListRowList());
            this.ListContext.GetKey = (x => x.Id);
            this.ListContext.ItemsSource.AddRange(this.States.Items);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.ListContainerContext.SetProcessingStates(false, this.ListContext.ItemsSource.Any());
            this.ListContext.SetProcessingStates(false);
            this.SearchContext.SetProcessingStates(false);
            this.CommandBarContext.SetProcessingStates(false);
            this.StateHasChanged();
        }
    }
    #endregion
}
