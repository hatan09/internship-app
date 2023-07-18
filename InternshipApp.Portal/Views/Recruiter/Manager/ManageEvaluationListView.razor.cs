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
    public FormRequest<FormAction, Evaluation> EvaluationFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<EvaluationListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected EvaluationListViewStates States { get; private set; }

    protected double AverageScore { get; private set; }
    protected PerformanceRank AveragePerformance { get; private set; }

    protected bool IsEditable { get; private set; }
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
        if (IsEditable)
        {
            EvaluationFormRequest = FormRequestFactory.EditRequest(rowItem.ToEntity());
            this.StateHasChanged();
        }
        else
        {
            EvaluationFormRequest = FormRequestFactory.EditRequest(rowItem.ToEntity());
        }
    }

    private void OnSelectionChanged()
    {
        var value = this.ListContext.GetSelectedItems().Any();
        this.CommandBarContext.SetItemIsVisible(ButtonNames.EditButton, value && IsEditable);
        this.CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, value && IsEditable);
        this.StateHasChanged();
    }


    #endregion

    #region [ Private Methods - Column ]
    private void InitializeColumn()
    {
        var project = new DataGridColumnDefinition<EvaluationListRowViewStates>("Project Title", x => x.ProjectName)
        {
            ColumnDataKey = nameof(EvaluationListRowViewStates.ProjectName),
            Width = "2fr"
        };

        var title = new DataGridColumnDefinition<EvaluationListRowViewStates>("Report Title", x => x.Title)
        {
            ColumnDataKey = nameof(EvaluationListRowViewStates.Title),
            Width = "3fr"
        };

        var performance = new DataGridColumnDefinition<EvaluationListRowViewStates>("Performance", x => x.Performance)
        {
            ColumnDataKey = nameof(EvaluationListRowViewStates.Performance),
            Width = "1fr"
        };

        var created = new DataGridColumnDefinition<EvaluationListRowViewStates>("Created Date", x => x.CreatedDate)
        {
            ColumnDataKey = nameof(EvaluationListRowViewStates.CreatedDate),
            Width = "1fr"
        };

        this.ListContext.Columns.Definitions.Add(project);
        this.ListContext.Columns.Definitions.Add(title);
        this.ListContext.Columns.Definitions.Add(performance);
        this.ListContext.Columns.Definitions.Add(created);
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
        this.CommandBarContext.Items.AddEditButton(this.OnEditButtonClicked, false);
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
        var item = new Evaluation
        {
            StudentId = this.StudentId,
            JobId = int.Parse(this.JobId),
        };
        EvaluationFormRequest = FormRequestFactory.AddRequest(item);
        StateHasChanged();
    }

    private void OnEditButtonClicked(EventArgs e)
    {
        var selectedItem = ListContext.GetSelectedItems().FirstOrDefault();
        if(selectedItem == null)
        {
            return;
        }
        this.EvaluationFormRequest = FormRequestFactory.AddRequest(selectedItem.ToEntity());
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
                Evaluations.Delete(item.ToEntity());

            }

            await Evaluations.SaveChangesAsync();
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
    protected async Task OnFormResultReceived(FormResult<Evaluation> result)
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
            this.ListContainerContext.SetProcessingStates(true, false);
            this.SearchContext.SetProcessingStates(true);
            this.CommandBarContext.SetProcessingStates(true);
            this.ListContext.SetProcessingStates(true);
            this.States.Items.Clear();
            this.StateHasChanged();

            if(!string.IsNullOrEmpty(JobId) && !string.IsNullOrEmpty(StudentId))
            {
                IsEditable = true;
                var evaluations = await Evaluations.FindAll(x => x.JobId == int.Parse(JobId) && x.StudentId == StudentId).AsNoTracking().ToListAsync();
                States.Items = evaluations.ToListRowList();
            }
            else if (!string.IsNullOrEmpty(JobId))
            {
                IsEditable = true;
                var evaluations = await Evaluations.FindAll(x => x.JobId == int.Parse(JobId)).AsNoTracking().ToListAsync();
                States.Items = evaluations.ToListRowList();
            }
            else if(!string.IsNullOrEmpty(StudentId))
            {
                IsEditable = false;
                this.CommandBarContext.SetItemIsVisible(ButtonNames.AddButton, false);
                var evaluations = await Evaluations.FindAll(x => x.StudentId == StudentId).AsNoTracking().ToListAsync();
                States.Items = evaluations.ToListRowList();
            }
            else
            {
                return;
            }

            AverageScore = States.Items.Sum(x => x.Score) / States.Items.Count;
            var totalPerformScore = States.Items.Sum(x => (long)Enum.Parse<PerformanceRank>(x.Performance));
            var averagePerformScore = totalPerformScore / States.Items.Count;
            if(totalPerformScore == 0)
            {
                AveragePerformance = PerformanceRank.AVERAGE;
            }
            else
            {
                if (averagePerformScore < 1)
                {
                    AveragePerformance = PerformanceRank.POOR;
                }
                else if (averagePerformScore < 2)
                {
                    AveragePerformance = PerformanceRank.AVERAGE;
                }
                else if (averagePerformScore < 3)
                {
                    AveragePerformance = PerformanceRank.GOOD;
                }
                else
                {
                    AveragePerformance = PerformanceRank.EXCELLENT;
                }
            }

            this.ListContext.GetKey = x => x.Id;
            this.ListContext.ItemsSource.Clear();
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