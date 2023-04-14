using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor.Lists;
using Wave5.UI;

namespace InternshipApp.Portal.Views;

public partial class CompanyListView
{
    #region [ Properties ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ICompanyRepository Companies { get; set; }

    public CompanyListViewStates States { get; set; }
    #endregion

    #region [ Private - Override Methods ]
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
        var filtered = new List<CompanyListRowViewStates>();

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

    private void OnFilterDataList(IEnumerable<CompanyListRowViewStates> filtered, string filterName = "")
    {
        //this.ListContext.ItemsSource.AddRange(filtered);

        if (!string.IsNullOrEmpty(filterName))
        {

        }
        this.StateHasChanged();
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(ClickEventArgs<CompanyListRowViewStates> args)
    {
        var selected = args.ItemData;
        if(selected != null)
            this.NavigationManager.NavigateTo($"/company/{selected.Id}");
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {
            this.States.Items.Clear();
            var companyList = new List<Company>();
            companyList.AddRange(await Companies.FindAll().ToListAsync());

            this.States.Items = companyList.ToListRowList();

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
