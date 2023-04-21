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
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IEvaluationRepository Evaluations { get; set; }

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
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = this.States.Items;
        }
        else
        {
            // have to check the string is not null first
            // for some reasons, the data can be null or blank
            filtered = this.States.Items.Where(x =>
                (string.IsNullOrEmpty(x.JobName) && x.JobName.Contains(value))
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

            var evaluations = await Evaluations.FindAll(x => x.JobId == int.Parse(JobId) && x.StudentId == StudentId).ToListAsync();
            States.Items = evaluations.ToListRowList();

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