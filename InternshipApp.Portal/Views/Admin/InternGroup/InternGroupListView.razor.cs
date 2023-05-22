using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class InternGroupListView
{
    #region [ Properties - Inject ]
    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public IInternGroupRepository InternGroups { get; set; }

    [Inject]
    public InstructorManager Instructors { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IInternGroupServices InternGroupServices { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, InternGroup> InternGroupFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<InternGroupListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected InternGroupListViewStates States { get; private set; }

    protected bool IsTeacherViewing { get; private set; }

    protected bool IsModalOpen { get; private set; }
    protected int MaxAmount { get; private set; } = 0;
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            States = new();

            SearchContext = new DataListSearchContext();
            ListContainerContext = new DetailsListContainerContext();
            ListContext = new DetailsListContext<InternGroupListRowViewStates>();
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
        var filtered = new List<InternGroupListRowViewStates>();

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
    private void OnRowClicked(InternGroupListRowViewStates rowItem)
    {
        NavigationManager.NavigateTo($"/group-info/{rowItem.Id}");
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
        var title = new DataGridColumnDefinition<InternGroupListRowViewStates>("Group Title", x => x.Title)
        {
            ColumnDataKey = nameof(InternGroupListRowViewStates.Title),
            Width = "3fr"
        };

        var instructor = new DataGridColumnDefinition<InternGroupListRowViewStates>("Group Manager", x => x.InstructorName)
        {
            ColumnDataKey = nameof(InternGroupListRowViewStates.InstructorName),
            Width = "3fr"
        };

        var amount = new DataGridColumnDefinition<InternGroupListRowViewStates>("Number of students", x => x.Amount)
        {
            ColumnDataKey = nameof(InternGroupListRowViewStates.Amount),
            Width = "2fr"
        };

        ListContext.Columns.Definitions.Add(instructor);
        ListContext.Columns.Definitions.Add(title);
        ListContext.Columns.Definitions.Add(amount);
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
        CommandBarContext.Items.AddButton("AutoCreate", "Auto-Create&Assign", "", OnAutoCreateButtonClicked);
        CommandBarContext.Items.AddButton("AutoAssign", "Auto-Assign", "", OnAutoAssignButtonClicked);
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
        var item = new InternGroup();
        InternGroupFormRequest = FormRequestFactory.AddRequest(item);
        StateHasChanged();
    }

    private void OnAutoCreateButtonClicked(EventArgs e)
    {
        IsModalOpen = true;
        StateHasChanged();
    }

    private void OnAutoAssignButtonClicked(EventArgs e)
    {
        StateHasChanged();
    }

    private void OnEditButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = ListContext.GetSelectedItems().FirstOrDefault();
            if (selectedItem == null || IsTeacherViewing)
            {
                return;
            }

            InternGroupFormRequest = FormRequestFactory.EditRequest(selectedItem.ToEntity());
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
            if (selectedItem.Count == 0 || IsTeacherViewing)
            {
                return;
            }

            foreach (var item in selectedItem)
            {
                InternGroups.Delete(item.ToEntity());

            }

            await InternGroups.SaveChangesAsync();
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
    protected async Task OnFormResultReceived(FormResult<InternGroup> result)
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
    private async Task<Instructor> GetInstructorAsync()
    {
        var user = await LocalStorage.GetItemAsync<User>("login-user-info");
        if (user == null)
        {
            NavigationManager.NavigateTo("/", true);
            return null;
        }

        return await Instructors.FindByIdAsync(user.Id);
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

            var groups = await InternGroups.FindAll().Include(x => x.Students).Include(x => x.Instructor).AsNoTracking().ToListAsync();
            groups.ForEach(x =>
            {
                var row = x.ToListRow();
                row.InstructorName = x.Instructor.FullName;
                row.Amount = x.Students.Count;
                States.Items.Add(row);
            });

            ListContext.GetKey = (x => x.Id);
            ListContext.ItemsSource.Clear();
            ListContext.ItemsSource.AddRange(States.Items);
            CommandBarContext.Items.FirstOrDefault(x => x.Name == "AutoCreate")!.IsVisible = !States.Items.Any();
            CommandBarContext.Items.FirstOrDefault(x => x.Name == "AutoAssign")!.IsVisible = States.Items.Any();
            if (States.Items.Any())
            {
                var sortCol = ListContext.Columns.Definitions.FirstOrDefault(x => x.ColumnDataKey == nameof(InternGroupListRowViewStates.Amount));
                if (sortCol != null)
                {
                    sortCol.IsSorted = true;
                    sortCol.IsAscending = true;
                }
            }
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

    public async void AutoCreateAndAssign()
    {
        await InternGroupServices.AutoCreateAndAssign(MaxAmount);
        await LoadDataAsync();
        IsModalOpen = false;
        await JSRuntime.InvokeVoidAsync("alert", "Creating process is done!");
        StateHasChanged();
    }
    #endregion
}
