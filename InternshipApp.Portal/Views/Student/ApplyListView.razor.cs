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
    //hire
    protected CommandBarContext HireViewCommandBarContext { get; private set; }

    protected DetailsCardContainerContext HireViewDetailsContainerContext { get; private set; }

    //apply
    public FormRequest<FormAction, StudentJob> ApplicationFormRequest { get; private set; }

    protected DetailsListContainerContext ApplyViewListContainerContext { get; private set; }

    protected DataListSearchContext ApplyViewSearchContext { get; private set; }

    protected CommandBarContext ApplyViewCommandBarContext { get; private set; }

    protected DetailsListContext<ApplicationListRowViewStates> ApplyViewListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected bool IsHired { get; set; } = false;
    protected ApplicationDetailsViewStates HireViewStates { get; private set; }
    protected ApplicationListViewStates ApplyViewStates { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            //hire
            HireViewStates = new();
            this.HireViewCommandBarContext = new CommandBarContext();
            this.HireViewDetailsContainerContext = new DetailsCardContainerContext();

            this.InitializeHireViewCommandBar();

            //apply
            ApplyViewStates = new();

            ApplyViewSearchContext = new();
            ApplyViewListContainerContext = new();
            ApplyViewListContext = new()
            {
                SelectionMode = SelectionMode.None
            };
            ApplyViewListContext.OnItemInvoked += OnRowClicked;

            InitializeApplyViewCommandBar();
            InitializeColumn();

            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {

        }
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentStudentId = this.StudentId;
        var parameterStudentId = parameters.GetValueOrDefault<string>(nameof(this.StudentId));

        await base.SetParametersAsync(parameters);

        if (currentStudentId != parameterStudentId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Event Handlers - Search ]
    private async void OnSearchDatalist(ChangeEventArgs args)
    {
        var filtered = new List<ApplicationListRowViewStates>();

        var value = args?.Value.ToString();
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = ApplyViewStates.Items;
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

        ApplyViewListContext.Columns.Definitions.Add(comp);
        ApplyViewListContext.Columns.Definitions.Add(job);
        ApplyViewListContext.Columns.Definitions.Add(stat);
    }
    #endregion

    #region [ Private Methods - CommandBars ]
    private void InitializeApplyViewCommandBar()
    {
        // Search
        ApplyViewSearchContext.OnDatalistSearch = OnSearchDatalist;

        // Items
        ApplyViewCommandBarContext = new CommandBarContext();
        ApplyViewCommandBarContext.Items.AddRefreshButton(OnApplyViewRefreshButtonClicked);

        //filter
        this.ApplyViewCommandBarContext.FarItems.AddFilterButton(new() {
            CommandBarFactory.CreateMenuItem("All", "All Applications", "", OnAllFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("Waiting", "Waiting Applications", "", OnWaitingFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("Accepted", "Accepted Applications", "", OnAcceptedFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("Rejected", "Rejected Applications", "", OnRejectedFilterButtonClicked),
        });
    }

    private void InitializeHireViewCommandBar()
    {
        // Items
        HireViewCommandBarContext = new CommandBarContext();
        HireViewCommandBarContext.Items.AddRefreshButton(OnHireViewRefreshButtonClicked);
    }
    #endregion

    #region [ Event Handlers - CommandBars - Buttons ]
    private async void OnApplyViewRefreshButtonClicked(EventArgs e)
    {
        ApplyViewSearchContext.SetDefafultSearchValue(string.Empty);
        await LoadDataAsync();
    }

    private async void OnHireViewRefreshButtonClicked(EventArgs e)
    {
        await LoadDataAsync();
    }

    private void OnAllFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.ApplyViewStates.Items, "All Applications");
    }

    private void OnWaitingFilterButtonClicked(MouseEventArgs obj)
    {
        var filtered = this.ApplyViewStates.Items.Where(x => x.Status == ApplyStatus.WAITING);
        this.OnFilterDataList(filtered, "Waiting Applications");
    }

    private void OnAcceptedFilterButtonClicked(MouseEventArgs obj)
    {
        var filtered = this.ApplyViewStates.Items.Where(x => x.Status == ApplyStatus.ACCEPTED);
        this.OnFilterDataList(filtered, "Accepted Applications");
    }

    private void OnRejectedFilterButtonClicked(MouseEventArgs obj)
    {
        var filtered = this.ApplyViewStates.Items.Where(x => x.Status == ApplyStatus.REJECTED);
        this.OnFilterDataList(filtered, "Rejected Applications");
    }

    private void OnFilterDataList(IEnumerable<ApplicationListRowViewStates> filtered, string filterName = "")
    {
        this.ApplyViewListContext.ClearSelectedItems();

        this.ApplyViewListContext.ItemsSource.Clear();
        this.ApplyViewListContext.ItemsSource.AddRange(filtered);
        this.ApplyViewListContext.Columns.Definitions.ForEach(x => x.IsSorted = false);
        this.ApplyViewListContainerContext.HasData = this.ApplyViewListContext.ItemsSource.Any();

        if (!string.IsNullOrEmpty(filterName))
        {
            this.ApplyViewCommandBarContext.FarItems.SetItemLabel(ButtonNames.FilterButton, filterName);
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

            HireViewDetailsContainerContext.SetProcessingStates(true, false);
            HireViewCommandBarContext.SetProcessingStates(true);

            ApplyViewListContainerContext.SetProcessingStates(true, false);
            ApplyViewSearchContext.SetProcessingStates(true);
            ApplyViewCommandBarContext.SetProcessingStates(true);
            ApplyViewListContext.SetProcessingStates(true);
            ApplyViewListContext.ItemsSource.Clear();
            ApplyViewStates.Items.Clear();

            StateHasChanged();

            var items = new List<StudentJob>();

            var student = await Students.FindAll(x => x.Id == StudentId)
                .AsNoTracking()
                .Include(x => x.StudentJobs)
                .FirstOrDefaultAsync();

            if(student == null)
            {
                HireViewStates = null;
                return;
            }

            IsHired = student.StudentJobs?.Where(x => x.Status == ApplyStatus.HIRED).Any() ?? false;

            if(IsHired)
            {
                var studentJob = student.StudentJobs?.Where(x => x.Status == ApplyStatus.HIRED).FirstOrDefault();
                if(studentJob == null)
                {
                    HireViewStates = null;
                    IsHired = false;
                    return;
                }

                var job = await Jobs.FindAll(x => x.Id == (studentJob.JobId ?? 0)).Include(x => x.Company).FirstOrDefaultAsync();
                if (job == null)
                {
                    HireViewStates = null;
                    IsHired = false;
                    return;
                }

                HireViewStates = new ApplicationDetailsViewStates()
                {
                    StudentId = student.Id,
                    JobId = job.Id,
                    StudentName = student.FullName,
                    JobName = job.Title,
                    CompanyName = job.Company?.Title
                };
            }
            else
            {
                items.AddRange(student.StudentJobs);

                var jobs = await Jobs.FindAll(x => student.StudentJobs.Select(x => x.JobId).Contains(x.Id))
                    .AsNoTracking()
                    .ToListAsync();

                var companies = await Companies.FindAll(x => jobs.Select(x => x.CompanyId).Contains(x.Id)).AsNoTracking().ToListAsync();

                ApplyViewStates.Items = items.ToListRowList();

                ApplyViewStates.Items.ForEach(x =>
                {
                    x.StudentName = student.FullName;
                    var job = jobs.FirstOrDefault(y => y.Id == x.JobId);
                    if (job != null)
                    {
                        x.JobName = job.Title;
                        x.CompanyName = companies.FirstOrDefault(y => y.Id == job.CompanyId)?.Title;
                    }
                });

                ApplyViewListContext.GetKey = (x => x.Id);
                OnFilterDataList(ApplyViewStates.Items, "All Applications");
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            HireViewDetailsContainerContext.SetProcessingStates(false, HireViewStates != null);
            HireViewCommandBarContext.SetProcessingStates(false);

            ApplyViewListContainerContext.SetProcessingStates(false, ApplyViewListContext.ItemsSource.Any());
            ApplyViewListContext.SetProcessingStates(false);
            ApplyViewSearchContext.SetProcessingStates(false);
            ApplyViewCommandBarContext.SetProcessingStates(false);
            StateHasChanged();
        }
    }
    #endregion
}
