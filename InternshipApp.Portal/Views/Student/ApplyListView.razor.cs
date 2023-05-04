using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;
using Wave5.UI;
using InternshipApp.Repository;
using Wave5.UI.Blazor;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Web;
using RCode;

namespace InternshipApp.Portal.Views;

public partial class ApplyListView
{
    #region [ Properties - Parameters ]
    [Parameter]
    public string StudentId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public ICompanyRepository Companies { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, StudentJob> ApplicationFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<ApplicationListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected ApplicationListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            States = new();

            SearchContext = new DataListSearchContext();
            ListContainerContext = new DetailsListContainerContext();
            ListContext = new DetailsListContext<ApplicationListRowViewStates>();
            ListContext.SelectionMode = SelectionMode.None;
            ListContext.OnItemInvoked += OnRowClicked;

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
        var filtered = new List<ApplicationListRowViewStates>();

        var value = args?.Value.ToString();
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = States.Items;
        }
        else
        {

        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(ApplicationListRowViewStates rowItem)
    {
        this.ApplicationFormRequest = FormRequestFactory.DetailsRequest(rowItem.ToEntity());
    }
    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
        var comp = new DataGridColumnDefinition<ApplicationListRowViewStates>("Company", x => x.CompanyName)
        {
            ColumnDataKey = nameof(ApplicationListRowViewStates.CompanyName),
            Width = "2fr"
        };

        var job = new DataGridColumnDefinition<ApplicationListRowViewStates>("Job", x => x.JobName)
        {
            ColumnDataKey = nameof(ApplicationListRowViewStates.JobName),
            Width = "2fr"
        };

        var stat = new DataGridColumnDefinition<ApplicationListRowViewStates>("Status", x => x.Status)
        {
            ColumnDataKey = nameof(ApplicationListRowViewStates.Status),
            Width = "1fr"
        };

        ListContext.Columns.Definitions.Add(comp);
        ListContext.Columns.Definitions.Add(job);
        ListContext.Columns.Definitions.Add(stat);
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

        //filter
        this.CommandBarContext.FarItems.AddFilterButton(new() {
            CommandBarFactory.CreateMenuItem("All", "All Applications", "", OnAllFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("Waiting", "Waiting Applications", "", OnWaitingFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("Accepted", "Accepted Applications", "", OnAcceptedFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("Rejected", "Rejected Applications", "", OnRejectedFilterButtonClicked),
        });
    }
    #endregion

    #region [ Event Handlers - CommandBars - Buttons ]
    private async void OnRefreshButtonClicked(EventArgs e)
    {
        SearchContext.SetDefafultSearchValue(string.Empty);
        await LoadDataAsync();
    }

    private void OnAllFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items, "All Applications");
    }

    private void OnWaitingFilterButtonClicked(MouseEventArgs obj)
    {
        var filtered = this.States.Items.Where(x => x.Status == ApplyStatus.WAITING);
        this.OnFilterDataList(filtered, "Waiting Applications");
    }

    private void OnAcceptedFilterButtonClicked(MouseEventArgs obj)
    {
        var filtered = this.States.Items.Where(x => x.Status == ApplyStatus.ACCEPTED);
        this.OnFilterDataList(filtered, "Accepted Applications");
    }

    private void OnRejectedFilterButtonClicked(MouseEventArgs obj)
    {
        var filtered = this.States.Items.Where(x => x.Status == ApplyStatus.REJECTED);
        this.OnFilterDataList(filtered, "Rejected Applications");
    }

    private void OnFilterDataList(IEnumerable<ApplicationListRowViewStates> filtered, string filterName = "")
    {
        this.ListContext.ClearSelectedItems();

        this.ListContext.ItemsSource.Clear();
        this.ListContext.ItemsSource.AddRange(filtered);
        this.ListContext.Columns.Definitions.ForEach(x => x.IsSorted = false);
        this.ListContainerContext.HasData = this.ListContext.ItemsSource.Any();

        if (!string.IsNullOrEmpty(filterName))
        {
            this.CommandBarContext.FarItems.SetItemLabel(ButtonNames.FilterButton, filterName);
        }
        this.StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            Guard.IsNullOrEmpty(StudentId, nameof(StudentId));

            ListContainerContext.SetProcessingStates(true, false);
            SearchContext.SetProcessingStates(true);
            CommandBarContext.SetProcessingStates(true);
            ListContext.SetProcessingStates(true);
            States.Items.Clear();

            StateHasChanged();

            var items = new List<StudentJob>();

            var student = await Students.FindAll(x => x.Id == StudentId)
                .AsNoTracking()
                .Include(x => x.StudentJobs.Where(x => x.Status == ApplyStatus.WAITING || 
                                                    x.Status == ApplyStatus.ACCEPTED || 
                                                    x.Status == ApplyStatus.REJECTED))
                .FirstOrDefaultAsync();

            if(student == null)
            {
                return;
            }

            items.AddRange(student.StudentJobs);

            var jobs = await Jobs.FindAll(x => student.StudentJobs.Select(x => x.JobId).Contains(x.Id))
                .AsNoTracking()
                .ToListAsync();

            var companies = await Companies.FindAll(x => jobs.Select(x => x.CompanyId).Contains(x.Id)).AsNoTracking().ToListAsync();

            States.Items = items.ToListRowList();

            States.Items.ForEach(x =>
            {
                x.StudentName = student.FullName;
                var job = jobs.FirstOrDefault(y => y.Id == x.JobId);
                if(job != null)
                {
                    x.JobName = job.Title;
                    x.CompanyName = companies.FirstOrDefault(y => y.Id == job.CompanyId)?.Title;
                }
            });

            ListContext.GetKey = (x => x.Id);
            OnFilterDataList(States.Items, "All Applications");
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
