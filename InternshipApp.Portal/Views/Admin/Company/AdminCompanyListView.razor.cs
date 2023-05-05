using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;
using Wave5.UI;
using Wave5.UI.Blazor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace InternshipApp.Portal.Views;

public partial class AdminCompanyListView
{
    #region [ Properties - Inject ]
    [Inject]
    public ICompanyRepository Companies { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, Company> CompanyFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<CompanyListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected CompanyListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            States = new();

            SearchContext = new DataListSearchContext();
            ListContainerContext = new DetailsListContainerContext();
            ListContext = new DetailsListContext<CompanyListRowViewStates>();
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
        var filtered = new List<CompanyListRowViewStates>();

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
                (!string.IsNullOrEmpty(x.Title) && x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(CompanyListRowViewStates rowItem)
    {
        NavigationManager.NavigateTo($"/admin-company-info/{rowItem.Id}");
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
        var title = new DataGridColumnDefinition<CompanyListRowViewStates>("Company Title", x => x.Title)
        {
            ColumnDataKey = nameof(CompanyListRowViewStates.Title),
            Width = "3fr"
        };

        ListContext.Columns.Definitions.Add(title);
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
        var item = new Company();
        CompanyFormRequest = FormRequestFactory.AddRequest(item);
        StateHasChanged();
    }

    private void OnEditButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = ListContext.GetSelectedItems().FirstOrDefault();
            if(selectedItem == null)
            {
                return;
            }

            CompanyFormRequest = FormRequestFactory.EditRequest(selectedItem.ToEntity());
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
                Companies.Delete(item.ToEntity());

            }

            await Companies.SaveChangesAsync();
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
    protected async Task OnFormResultReceived(FormResult<Company> result)
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
        var company = await Companies.FindAll().AsNoTracking().FirstOrDefaultAsync();
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

            var items = await Companies.FindAll().AsNoTracking().ToListAsync();

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
