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

public partial class ManageInternListView
{
    #region [ Properties - Parameters ]
    [Parameter]
    public string? JobId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public ILogger<ApplicationListView> Logger { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public StudentManager Students { get; set; }
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
            this.States = new();

            this.SearchContext = new DataListSearchContext();
            this.ListContainerContext = new DetailsListContainerContext();
            this.ListContext = new DetailsListContext<ApplicationListRowViewStates>();
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
        var filtered = new List<ApplicationListRowViewStates>();

        var value = args?.Value.ToString();
        var isNumber = double.TryParse(value, out var doubleValue);
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = this.States.Items;
        }
        else
        {
            // have to check the string is not null first
            // for some reasons, the data can be null or blank
            filtered = this.States.Items.Where(x =>
                (isNumber && x.Matching >= doubleValue) ||
                (string.IsNullOrEmpty(x.StudentName) && x.StudentName.Contains(value)) ||
                (string.IsNullOrEmpty(x.JobName) && x.JobName.Contains(value))
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(ApplicationListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/job/{rowItem.JobId}/student/{rowItem.StudentId}");
    }

    private void OnSelectionChanged()
    {
        var value = this.ListContext.GetSelectedItems().Any();
        CommandBarContext.SetItemIsVisible("Details", value);
        this.StateHasChanged();
    }
    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
        var job = new DataGridColumnDefinition<ApplicationListRowViewStates>("Job Title", x => x.JobName)
        {
            ColumnDataKey = nameof(ApplicationListRowViewStates.JobName),
            Width = "2fr"
        };

        var name = new DataGridColumnDefinition<ApplicationListRowViewStates>("Student Name", x => x.StudentName)
        {
            ColumnDataKey = nameof(ApplicationListRowViewStates.StudentName),
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
        this.CommandBarContext.Items.AddButton("Details", "Details", "Info", this.OnDetailsButtonClicked);
    }
    #endregion

    #region [ Event Handlers - CommandBars - Buttons ]
    private async void OnRefreshButtonClicked(EventArgs e)
    {
        this.SearchContext.SetDefafultSearchValue(string.Empty);
        await this.LoadDataAsync();
    }

    private void OnDetailsButtonClicked(EventArgs e)
    {
        var item = ListContext.GetSelectedItems().FirstOrDefault();
        if(item != null)
        {
            ApplicationFormRequest = FormRequestFactory.DetailsRequest(item.ToEntity());
            StateHasChanged();
        }
    }
    #endregion

    #region [ Private Methods - Data ]
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

            var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).Include(x => x.StudentJobs).FirstOrDefaultAsync();

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