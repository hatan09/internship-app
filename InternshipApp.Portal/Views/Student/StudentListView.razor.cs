using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using System.Xml.Linq;
using Wave5.UI;
using Wave5.UI.Blazor;
using Wave5.UI.DataGrids;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class StudentListView : ComponentBase
{
    #region [ Properties - Inject ]
    [Inject]
    public ILogger<StudentListView> Logger { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
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

        this.ListContext.Columns.Definitions.Add(id);
        this.ListContext.Columns.Definitions.Add(name);
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
        this.CommandBarContext.Items.AddAddButton(this.OnAddButtonClicked);
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
            certificationList.Add(new()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Tan Ha",
                StudentId = "ITITIU18184"
            });

            this.States.Items.AddRange(certificationList.ToListRowList());
            this.ListContext.GetKey = (x => x.Id);
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
            this.CommandBarContext.SetItemIsVisible(ButtonNames.DeleteButton, this.ListContext.GetSelectedItems().Any());
            this.StateHasChanged();
        }
    }
    #endregion
}
