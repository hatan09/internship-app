using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Lists;

namespace InternshipApp.Portal.Views;
public partial class JobListView
{
    #region
    public JobListViewStates States { get; set; }

    public SfMultiSelect<string[], Option> SfMultiSelect { get; set; }
    #endregion

    #region
    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public ICompanyRepository Companies { get; set; }

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
        var value = args?.Value.ToString();
        if (String.IsNullOrWhiteSpace(value))
        {

        }
        else
        {

        }
    }
    #endregion

    #region [ Event Handlers - CommandBars - Filters ]
    private async void FilterChangeHandler()
    {
        await OnFilterItems();
    }

    private List<JobListRowViewStates> GetItemsList(List<Option> options)
    {
        var result = new List<JobListRowViewStates>();
        options.ForEach(x =>
        {
            result.AddRange(States.OriginalItems.Where(y => y.Id == int.Parse(x.Id)).ToList());
        });

        return result;
    }

    public async Task OnFilterItems()
    {
        var items = await this.SfMultiSelect.GetItemsAsync();
        if (!items.Any())
        {
            States.Items = States.OriginalItems;
            return;
        }
        var filtered = GetItemsList(items.ToList());
        States.Items = filtered;
    }
    #endregion

    #region [ Event Handlers - DataList ]
    private void OnRowClicked(JobListRowViewStates rowItem)
    {
        this.NavigationManager.NavigateTo($"/job-details/{rowItem.Id}");
    }
    #endregion

    #region [ Private Methods - Data ]
    private void OnRowSelect(ClickEventArgs<JobListRowViewStates> args)
    {
        var selected = args.ItemData;
        if (selected != null)
            NavigationManager.NavigateTo($"/job/{selected.Id}");
    }

    private async Task LoadDataAsync()
    {
        try
        {
            this.States.Items.Clear();
            this.States.Options.Clear();
            var jobList = new List<Job>();
            jobList.AddRange(await Jobs.FindAll().Include(x => x.Company).ToListAsync());

            var skills = await Skills.FindAll().ToListAsync();
            skills.ForEach(x =>
            {
                States.Options.Add(
                    new()
                    {
                        Id = x.Id.ToString(),
                        Category = x.Type.ToString(),
                        Title = x.Name
                    }
                );
            });

            this.States.Items = jobList.ToListRowList();
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
