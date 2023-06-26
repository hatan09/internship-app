using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class StudentListView : ComponentBase
{
    #region [ Properties - Param ]
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

    [Inject]
    public IJSRuntime JSRuntime { get; set; }
    #endregion

    #region [ Properties - States - Contexts ]
    public FormRequest<FormAction, Student> StudentFormRequest { get; private set; }

    protected DetailsListContainerContext ListContainerContext { get; private set; }

    protected DataListSearchContext SearchContext { get; private set; }

    protected CommandBarContext CommandBarContext { get; private set; }

    protected DetailsListContext<StudentListRowViewStates> ListContext { get; private set; }

    //no-group
    public FormRequest<FormAction, Student> NoGroupStudentFormRequest { get; private set; }

    protected DetailsListContainerContext NoGroupListContainerContext { get; private set; }

    protected DataListSearchContext NoGroupSearchContext { get; private set; }

    protected CommandBarContext NoGroupCommandBarContext { get; private set; }

    protected DetailsListContext<StudentListRowViewStates> NoGroupListContext { get; private set; }
    #endregion

    #region [ Properties - States - DataList ]
    protected bool IsManagingGroup { get; set; }
    protected StudentListViewStates States { get; private set; }
    protected StudentListViewStates NoGroupStates { get; private set; }
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
            this.ListContext.SelectionMode = SelectionMode.Single;
            this.ListContext.OnItemInvoked += this.OnRowClicked;
            this.ListContext.OnSelectionChanged += this.OnSelectionChanged;

            //no-group
            this.NoGroupStates = new StudentListViewStates();

            this.NoGroupSearchContext = new DataListSearchContext();
            this.NoGroupListContainerContext = new DetailsListContainerContext();
            this.NoGroupListContext = new DetailsListContext<StudentListRowViewStates>();
            this.NoGroupListContext.SelectionMode = SelectionMode.Multiple;
            this.NoGroupListContext.OnItemInvoked += this.OnRowClicked;
            this.NoGroupListContext.OnSelectionChanged += this.OnNoGroupSelectionChanged;

            this.InitializeCommandBar();
            this.InitializeNoGroupCommandBar();
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
            IsTeacherViewing = (await LocalStorage.GetItemAsStringAsync("role")) == "INSTRUCTOR";
            await this.OnInitializedAsync();
            await this.LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Event Handlers - Search ]
    private void OnSearchDatalist(ChangeEventArgs args)
    {
        var filtered = new List<StudentListRowViewStates>();

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
                (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.StudentId) && x.StudentId.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }
    }

    private void OnNoGroupSearchDatalist(ChangeEventArgs args)
    {
        var filtered = new List<StudentListRowViewStates>();

        var value = args?.Value.ToString();
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = this.NoGroupStates.Items;
        }
        else
        {
            // have to check the string is not null first
            // for some reasons, the data can be null or blank
            filtered = this.NoGroupStates.Items.Where(x =>
                (!string.IsNullOrEmpty(x.Name) && x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.StudentId) && x.StudentId.Contains(value, StringComparison.InvariantCultureIgnoreCase))
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
        this.CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, value && !IsTeacherViewing);
        this.StateHasChanged();
    }

    private void OnNoGroupSelectionChanged()
    {
        var value = this.NoGroupListContext.GetSelectedItems().Any();
        this.NoGroupCommandBarContext.SetItemIsVisible("AddToGroupButton", value && IsTeacherViewing && IsManagingGroup);
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

        this.NoGroupListContext.Columns.Definitions.Add(id);
        this.NoGroupListContext.Columns.Definitions.Add(name);
        this.NoGroupListContext.Columns.Definitions.Add(group);
        this.NoGroupListContext.Columns.Definitions.Add(status);
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

        if (!IsTeacherViewing)
        {
            this.CommandBarContext.Items.AddAddButton(this.OnAddButtonClicked);
            this.CommandBarContext.Items.AddDeleteButton(this.OnDeleteButtonClicked, false);

            this.CommandBarContext.FarItems.AddFilterButton(new() {
                CommandBarFactory.CreateMenuItemFilterAll(this.OnAllFilterButtonClicked),
                CommandBarFactory.CreateMenuItem("HasGroup", "HasGroup", "SkypeCircleCheck", this.OnHasGroupFilterButtonClicked),
                CommandBarFactory.CreateMenuItem("NoGroup", "NoGroup", "SkypeCircleMinus", this.OnNoGroupFilterButtonClicked),
                CommandBarFactory.CreateMenuItem("Rejected", "Rejected", "UserWarning", this.OnRejectedFilterButtonClicked),
            });
        }
        else
        {
            this.CommandBarContext.FarItems.AddFilterButton(new() {
                CommandBarFactory.CreateMenuItemFilterAll(this.OnAllFilterButtonClicked),
                CommandBarFactory.CreateMenuItem("Waiting", "Waiting", "Clock", this.OnWaitingFilterButtonClicked),
                CommandBarFactory.CreateMenuItem("Applied", "Applied", "Feedback", this.OnAppliedFilterButtonClicked),
                CommandBarFactory.CreateMenuItem("Hired", "Hired", "Commitments", this.OnHiredFilterButtonClicked),
                CommandBarFactory.CreateMenuItem("Finished", "Finished", "Medal", this.OnFinishedFilterButtonClicked),
            });
        }
        
    }

    private void InitializeNoGroupCommandBar()
    {
        // Search
        this.NoGroupSearchContext.OnDatalistSearch = this.OnNoGroupSearchDatalist;

        // Items
        this.NoGroupCommandBarContext = new CommandBarContext();
        this.NoGroupCommandBarContext.Items.AddRefreshButton(this.OnRefreshButtonClicked);
        this.NoGroupCommandBarContext.Items.AddButton("AddToGroupButton", "Add To My Group", "PeopleAdd", this.OnAddToGroupButtonClicked);

        this.NoGroupCommandBarContext.FarItems.AddFilterButton(new() {
            CommandBarFactory.CreateMenuItemFilterAll(this.OnAllFilterButtonClicked),
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

    private void OnRejectedFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items.Where(x => x.Status == Stat.REJECTED.ToString()), "Rejected Students");
    }

    private void OnWaitingFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items.Where(x => x.Status == Stat.WAITING.ToString()), "Waiting Students");
    }

    private void OnAppliedFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items.Where(x => x.Status == Stat.APPLIED.ToString()), "Applied Students");
    }

    private void OnHiredFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items.Where(x => x.Status == Stat.HIRED.ToString()), "Hired Students");
    }

    private void OnFinishedFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items.Where(x => x.Status == Stat.FINISHED.ToString()), "Finished Students");
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

    private async void OnAddToGroupButtonClicked(EventArgs e)
    {
        try
        {
            var selectedItems = this.NoGroupListContext.GetSelectedItems();
            if (selectedItems.Count == 0)
            {
                return;
            }
            var ids = selectedItems.Select(x => x.Id).ToList();


            var instructor = await GetInstructorAsync();
            if(instructor == null)
            {
                NavigationManager.NavigateTo("/", true);
                return;
            }
            var group = await Groups.FindAll(x => x.InstructorId == instructor.Id).Include(x => x.Students).AsTracking().FirstOrDefaultAsync();
            if(group == null)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Cannot find your group.");
                return;
            }

            var students = await Students.FindAll(x => ids.Contains(x.Id)).AsTracking().ToListAsync();
            foreach(var student in students)
            {
                group.Students.Add(student);
            }

            Groups.Update(group);
            await Groups.SaveChangesAsync();

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

        return await Instructors.FindAll(x => x.Id == user.Id).Include(x => x.InternGroup).FirstOrDefaultAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            this.ListContainerContext.SetProcessingStates(true, false);
            this.SearchContext.SetProcessingStates(true);
            this.CommandBarContext.SetProcessingStates(true);
            this.ListContext.SetProcessingStates(true);
            this.ListContext.SelectionMode = IsTeacherViewing ? SelectionMode.Single : SelectionMode.Multiple;
            this.States.Items.Clear();

            this.StateHasChanged();

            //instructor
            if (IsTeacherViewing)
            {
                var instructor = await GetInstructorAsync();
                if (instructor == null)
                {
                    NavigationManager.NavigateTo("/", true);
                    return;
                }
                IsManagingGroup = instructor.InternGroup != null;

                if (IsManagingGroup)
                {
                    var studentList = new List<Student>();
                    var group = await Groups.FindAll(x => x.InstructorId == instructor.Id).Include(x => x.Students.Where(x => x.Stat != Stat.REJECTED)).AsNoTracking().FirstOrDefaultAsync();
                    if (group != null)
                    {
                        studentList.AddRange(group.Students);
                        States.Items.AddRange(studentList.ToListRowList());
                        States.Items.ForEach(x =>
                        {
                            x.InternGroupName = group.Title;
                        });
                    }

                    await LoadNoGroupDataAsync();
                }
                else
                {
                    this.NoGroupListContext.GetKey = x => x.Id;
                    this.NoGroupListContext.ItemsSource.Clear();
                }
            }
            //admin
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
                    var students = await Students.FindAll(x => x.InternGroupId == InternGroupId).AsNoTracking().Include(x => x.InternGroup).ToListAsync();
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

    private async Task LoadNoGroupDataAsync()
    {
        try
        {
            this.NoGroupListContainerContext.SetProcessingStates(true, false);
            this.NoGroupSearchContext.SetProcessingStates(true);
            this.NoGroupCommandBarContext.SetProcessingStates(true);
            this.NoGroupListContext.SetProcessingStates(true);
            this.NoGroupStates.Items.Clear();

            this.StateHasChanged();

            var studentList = await Students.FindAll(x => x.InternGroup == null && x.Stat != Stat.REJECTED).AsNoTracking().ToListAsync();
            NoGroupStates.Items = studentList.ToListRowList();

            this.NoGroupListContext.GetKey = x => x.Id;
            this.NoGroupListContext.ItemsSource.Clear();
            this.NoGroupListContext.ItemsSource.AddRange(NoGroupStates.Items);
        }
        catch (Exception ex) 
        {
        
        }
        finally
        {
            this.NoGroupListContainerContext.SetProcessingStates(false, this.NoGroupListContext.ItemsSource.Any());
            this.NoGroupListContext.SetProcessingStates(false);
            this.NoGroupSearchContext.SetProcessingStates(false);
            this.NoGroupCommandBarContext.SetProcessingStates(false);
            this.NoGroupCommandBarContext.SetItemIsVisible("AddToGroupButton", false);
            this.StateHasChanged();
        }
    }
    #endregion
}
