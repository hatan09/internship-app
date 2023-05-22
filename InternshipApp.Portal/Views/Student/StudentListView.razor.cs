using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class StudentListView : ComponentBase
{
    #region [ Properties ]
    [Parameter]
    public int? InternGroupId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public IInternGroupRepository Groups { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public InstructorManager Instructors { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, Student> StudentFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<StudentListRowViewStates> ListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected StudentListViewStates States { get; private set; }
    protected bool IsTeacherViewing { get; set; }
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
            this.ListContext.SelectionMode = SelectionMode.Multiple;
            this.ListContext.OnItemInvoked += this.OnRowClicked;
            this.ListContext.OnSelectionChanged += this.OnSelectionChanged;

            IsTeacherViewing = (await LocalStorage.GetItemAsStringAsync("role")) == "INSTRUCTOR";

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
        var filtered = new List<StudentListRowViewStates>();

        //await this.AppLogicProvider.InvokeSearchDelayAsync();

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
                (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(StudentListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/student/{rowItem.Id}");
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
        var id = new DataGridColumnDefinition<StudentListRowViewStates>("Student ID", x => x.StudentId)
        {
            ColumnDataKey = nameof(StudentListRowViewStates.StudentId),
            Width = "1fr"
        };

        var name = new DataGridColumnDefinition<StudentListRowViewStates>("Student Name", x => x.Name)
        {
            ColumnDataKey = nameof(StudentListRowViewStates.Name),
            Width = "2fr"
        };

        var status = new DataGridColumnDefinition<StudentListRowViewStates>("Status", x => x.Status)
        {
            ColumnDataKey = nameof(StudentListRowViewStates.Status),
            Width = "1fr"
        };

        var group = new DataGridColumnDefinition<StudentListRowViewStates>("Group", x => string.IsNullOrEmpty(x.InternGroupName) ? "No Group" : x.InternGroupName)
        {
            ColumnDataKey = nameof(StudentListRowViewStates.InternGroupName),
            Width = "2fr"
        };

        this.ListContext.Columns.Definitions.Add(id);
        this.ListContext.Columns.Definitions.Add(name);
        this.ListContext.Columns.Definitions.Add(group);
        this.ListContext.Columns.Definitions.Add(status);
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
        if (InternGroupId == null)
        {
            this.CommandBarContext.Items.AddAddButton(this.OnAddButtonClicked);
            this.CommandBarContext.Items.AddDeleteButton(this.OnDeleteButtonClicked, false);
        }
        
        this.CommandBarContext.FarItems.AddFilterButton(new() {
            CommandBarFactory.CreateMenuItemFilterAll(this.OnAllFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("HasGroup", "HasGroup", "", this.OnHasGroupFilterButtonClicked),
            CommandBarFactory.CreateMenuItem("NoGroup", "NoGroup", "", this.OnNoGroupFilterButtonClicked),
        });
    }
    #endregion

    #region [ Event Handlers - CommandBars - Filters ]
    private void OnAllFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items, MenuItemNames.FilterAll);
    }

    private void OnHasGroupFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items.Where(x => x.InternGroupId > 0), "HasGroup");
    }

    private void OnNoGroupFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items.Where(x => x.InternGroupId <= 0), "NoGroup");
    }

    private void OnFilterDataList(IEnumerable<StudentListRowViewStates> filtered, string filterName = "")
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

    #region [ Event Handlers - CommandBars - Buttons ]
    private async void OnRefreshButtonClicked(EventArgs e)
    {
        this.SearchContext.SetDefafultSearchValue(string.Empty);
        await this.LoadDataAsync();
    }

    private void OnAddButtonClicked(EventArgs e)
    {
        var item = new Student();
        StudentFormRequest = FormRequestFactory.AddRequest(item);
        this.StateHasChanged();
    }

    private void OnEditButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = this.ListContext.GetSelectedItems();
            if (selectedItem.Count == 0)
            {
                return;
            }

            var item = selectedItem.FirstOrDefault();
            this.StudentFormRequest = FormRequestFactory.EditRequest(item.ToEntity());

            this.StateHasChanged();
        }
        catch (Exception ex)
        {

        }
    }

    private async void OnDeleteButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItem = this.ListContext.GetSelectedItems();
            if (selectedItem.Count == 0)
            {
                return;
            }

            foreach (var item in selectedItem)
            {
                var student = await Students.FindByIdAsync(item.Id);
                if (student != null)
                {
                    await Students.DeleteAsync(student);
                }

            }
            var tasks = new List<Task>();

            tasks.Add(this.LoadDataAsync());

            await Task.WhenAll(tasks);

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<Student> result)
    {
        switch (result.State)
        {
            case FormResultState.Added:
            case FormResultState.Updated:
            case FormResultState.Deleted:
                var tasks = new List<Task>
                {
                    this.LoadDataAsync()
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
            return null;
        }

        return await Instructors.FindByIdAsync(user.Id);
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

            if (IsTeacherViewing)
            {
                var instructor = await GetInstructorAsync();
                if (instructor == null)
                {
                    return;
                }
                var studentList = new List<Student>();
                var group = await Groups.FindAll(x => x.InstructorId == instructor.Id).Include(x => x.Students).AsNoTracking().FirstOrDefaultAsync();
                if (group == null)
                {
                    return;
                }
                studentList.AddRange(group.Students);
                States.Items.AddRange(studentList.ToListRowList());
                States.Items.ForEach(x =>
                {
                    x.InternGroupName = group.Title;
                });
            }
            else
            {
                if (InternGroupId == null)
                {
                    var studentList = new List<Student>();
                    var students = await Students.FindAll().AsNoTracking().Include(x => x.InternGroup).ToListAsync();
                    students.ForEach(x =>
                    {
                        var row = x.ToListRow();
                        row.InternGroupName = x.InternGroup?.Title;
                        this.States.Items.Add(row);
                    });
                }
                else
                {
                    var students = await Students.FindAll(x => x.InternGroupId == (InternGroupId?? 0)).AsNoTracking().Include(x => x.InternGroup).ToListAsync();
                    students.ForEach(x =>
                    {
                        var row = x.ToListRow();
                        row.InternGroupName = x.InternGroup?.Title;
                        this.States.Items.Add(row);
                    });
                }
            }


            this.ListContext.GetKey = x => x.Id;
            this.ListContext.ItemsSource.Clear();
            this.OnFilterDataList(this.States.Items, MenuItemNames.FilterAll);
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
            this.CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, this.ListContext.GetSelectedItems().Any());
            this.StateHasChanged();
        }
    }
    #endregion
}
