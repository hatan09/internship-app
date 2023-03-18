using Microsoft.AspNetCore.Components;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.DataGrids;

namespace InternshipApp.Portal.Views;

public partial class StudentListView : ComponentBase
{
    #region [ CTor ]
    public StudentListView()
    {

    }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public ILogger<StudentListView> Logger { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<StudentListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected StudentListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            this.States = new StudentListViewStates();

            this.SearchContext = new DataListSearchContext();
            this.ListContainerContext = new DetailsListContainerContext();
            this.ListContext = new DetailsListContext<StudentListRowViewStates>();
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

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(StudentListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/certification-details/{rowItem.Id}");
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

        this.CommandBarContext.FarItems.AddFilterButton(new() {
            CommandBarFactory.CreateMenuItemFilterAll(this.OnAllFilterButtonClicked),
            CommandBarFactory.CreateMenuItemFilterActive(this.OnActiveFilterButtonClicked),
            CommandBarFactory.CreateMenuItemFilterInActive(this.OnInActiveFilterButtonClicked),
        });
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

            var certificationList = new List<Student>();
            certificationList = await this.Logic.Students.GetAllAsync();
            this.States.Items = certificationList.ToListRowList();

            this.ListContext.GetKey = (x => x.Id);

            this.OnFilterDataList(this.States.Items, MenuItemNames.FilterAll);
            // TODO: await this.AppLogicProvider.InvokeLoadDelayAsync();
        }
        catch (Exception ex)
        {
            this.Logger.LogException(ex);
            // TODO: this.MessageProvider.AddExceptionMessage(this, ex);
        }
        finally
        {
            this.ListContainerContext.SetProcessingStates(false, this.ListContext.ItemsSource.Any());
            this.ListContext.SetProcessingStates(false);
            this.SearchContext.SetProcessingStates(false);
            this.CommandBarContext.SetProcessingStates(false);
            this.CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, this.ListContext.GetSelectedItems().Any());
            this.StateHasChanged();
        }
    }
    #endregion
}
