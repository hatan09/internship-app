using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;
using Wave5.UI;
using Wave5.UI.Blazor;
using InternshipApp.Contracts;
using InternshipApp.Repository;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class ManageEvaluationListView
{
    #region [ Properties - Parameters ]
    [Parameter]
    public string? JobId { get; set; }

    [Parameter]
    public string? StudentId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public ILogger<EvaluationListView> Logger { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IMatchingService MatchingService { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, StudentJob> EvaluationFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<EvaluationListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected EvaluationListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            this.States = new();

            this.SearchContext = new DataListSearchContext();
            this.ListContainerContext = new DetailsListContainerContext();
            this.ListContext = new DetailsListContext<EvaluationListRowViewStates>();
            this.ListContext.SelectionMode = SelectionMode.Single;
            this.ListContext.OnItemInvoked += this.OnRowClicked;
            this.ListContext.OnSelectionChanged += this.OnSelectionChanged;

            this.InitializeCommandBar();
            this.InitializeColumn();

            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex.ToString());
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
        var filtered = new List<EvaluationListRowViewStates>();

        var value = args?.Value.ToString();
        double.TryParse(value, out var doubleValue);
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = this.States.Items;
        }
        else
        {
            // have to check the string is not null first
            // for some reasons, the data can be null or blank
            filtered = this.States.Items.Where(x =>
                (x.Matching > 0 && x.Matching >= doubleValue)
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(EvaluationListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/manage-itern/{rowItem.StudentId}");
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
        var job = new DataGridColumnDefinition<EvaluationListRowViewStates>("Job Title", x => x.JobName)
        {
            ColumnDataKey = nameof(EvaluationListRowViewStates.JobName),
            Width = "2fr"
        };

        var name = new DataGridColumnDefinition<EvaluationListRowViewStates>("Student Name", x => x.StudentName)
        {
            ColumnDataKey = nameof(EvaluationListRowViewStates.StudentName),
            Width = "2fr"
        };

        var title = new DataGridColumnDefinition<EvaluationListRowViewStates>("Title", x => x.StudentName)
        {
            ColumnDataKey = nameof(EvaluationListRowViewStates.StudentName),
            Width = "2fr"
        };
        this.ListContext.Columns.Definitions.Add(job);
        this.ListContext.Columns.Definitions.Add(name);
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
    }
    #endregion

    #region [ Event Handlers - CommandBars - Buttons ]
    private async void OnRefreshButtonClicked(EventArgs e)
    {
        this.SearchContext.SetDefafultSearchValue(string.Empty);
        await this.LoadDataAsync();
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task<int> GetCompanyId()
    {
        return 1;
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

            var companyId = await GetCompanyId();

            var job = new Job();

            if (string.IsNullOrEmpty(JobId))
            {
                job = await Jobs.FindAll(x => x.CompanyId == companyId)
                                .Include(x => x.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED))
                                .FirstOrDefaultAsync();
            }
            else
            {
                job = await Jobs.FindAll(x => x.Id == int.Parse(JobId))
                                .Include(x => x.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED)) 
                                .FirstOrDefaultAsync();
            }

            var studentJobs = job?.StudentJobs.ToList();

            States.Items = studentJobs.ToListRowList();
            var allStudents = await Students.FindAll(x => States.Items.Select(y => y.StudentId).Contains(x.Id))
                                        .ToListAsync();

            States.Items.ForEach(x => {
                var student = allStudents.FirstOrDefault(y => y.Id == x.StudentId);

                x.StudentName = student == null ? "" : student.FullName;
                x.JobName = job.Title;
            });

            this.ListContext.GetKey = x => x.Id;
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