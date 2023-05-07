using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;
using Wave5.UI;
using InternshipApp.Repository;
using Wave5.UI.Blazor;
using Microsoft.EntityFrameworkCore;
using RCode;

namespace InternshipApp.Portal.Views;

public partial class RecruiterListView
{
    #region [ Properties - Parameters ]
    [Parameter]
    public int CompanyId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public RecruiterManager Recruiters { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, Recruiter> RecruiterFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<RecruiterListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected RecruiterListViewStates States { get; private set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            States = new();

            SearchContext = new DataListSearchContext();
            ListContainerContext = new DetailsListContainerContext();
            ListContext = new DetailsListContext<RecruiterListRowViewStates>();
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
        var filtered = new List<RecruiterListRowViewStates>();

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
                (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(RecruiterListRowViewStates rowItem)
    {
        RecruiterFormRequest = FormRequestFactory.EditRequest(rowItem.ToEntity());
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
        var name = new DataGridColumnDefinition<RecruiterListRowViewStates>("Recruiter Name", x => x.Name)
        {
            ColumnDataKey = nameof(RecruiterListRowViewStates.Name),
            Width = "3fr"
        };

        var username = new DataGridColumnDefinition<RecruiterListRowViewStates>("Username", x => x.Username)
        {
            ColumnDataKey = nameof(RecruiterListRowViewStates.Username),
            Width = "3fr"
        };

        ListContext.Columns.Definitions.Add(name);
        ListContext.Columns.Definitions.Add(username);
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
        var item = new Recruiter
        {
            CompanyId = CompanyId
        };
        RecruiterFormRequest = FormRequestFactory.AddRequest(item);
        StateHasChanged();
    }

    private void OnEditButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = ListContext.GetSelectedItems().FirstOrDefault();
            if (selectedItem == null)
            {
                return;
            }

            RecruiterFormRequest = FormRequestFactory.EditRequest(selectedItem.ToEntity());
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
                var recruiter = await Recruiters.FindByIdAsync(item.Id);
                if(recruiter != null)
                {
                    await Recruiters.DeleteAsync(recruiter);
                }
            }

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
    protected async Task OnFormResultReceived(FormResult<Recruiter> result)
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
            Guard.ParamIsNull(CompanyId, nameof(CompanyId));

            ListContainerContext.SetProcessingStates(true, false);
            SearchContext.SetProcessingStates(true);
            CommandBarContext.SetProcessingStates(true);
            ListContext.SetProcessingStates(true);
            States.Items.Clear();

            StateHasChanged();

            var items = await Recruiters.FindAll(x => x.CompanyId == CompanyId).AsNoTracking().ToListAsync();

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
