using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Wave5.UI;

namespace InternshipApp.Portal.Views; 
public partial class JobListView {
    #region
    public JobListViewStates States { get; set; }
    #endregion

    #region
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region
    protected override async Task OnInitializedAsync()
    {
        States = new();
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
    private void OnSearchDatalist(ChangeEventArgs args)
    {
        var filtered = new List<JobListRowViewStates>();

        var value = args?.Value.ToString();
        if (String.IsNullOrWhiteSpace(value))
        {
            filtered = this.States.Items;
        }
        else
        {
            
        }

        this.OnFilterDataList(filtered);
    }
    #endregion

    #region [ Event Handlers - CommandBars - Filters ]
    private void OnAllFilterButtonClicked(MouseEventArgs obj)
    {
        this.OnFilterDataList(this.States.Items, MenuItemNames.FilterAll);
    }

    private void OnFilterDataList(IEnumerable<JobListRowViewStates> filtered, string filterName = "")
    {
        //this.ListContext.ItemsSource.AddRange(filtered);

        if (!string.IsNullOrEmpty(filterName))
        {

        }
        this.StateHasChanged();
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(JobListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/job-details/{rowItem.Id}");
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            this.States.Items.Clear();
            var jobList = new List<Job>()
            {
                new Job(),
                new Job(),
                new Job(),
            };

            this.States.Items = jobList.ToListRowList();
            this.States.Items.ForEach(x => x.Title = "C# dev");
            this.States.Items.ForEach(x => x.CompanyName = "iDealogic");
            this.States.Items.ForEach(x => x.Description = "C# dev");

            //this.OnFilterDataList(this.States.Items, MenuItemNames.FilterAll);
            // TODO: await this.AppLogicProvider.InvokeLoadDelayAsync();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.StateHasChanged();
        }
    }
    #endregion
}
