using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Lists;

namespace InternshipApp.Portal.Views;
public partial class JobListView
{
    #region [ Properties ]
    [Parameter]
    public string CompanyId { get; set; }

    public JobListViewStates States { get; set; }

    public SfMultiSelect<string[], JobFilterOption> SfMultiSelect { get; set; }

    public string SearchText { get; set; }

    public bool IsStudentViewing { get; set; }
    public bool IsTeacherViewing { get; set; }
    public bool IsRecruiterViewing { get; set; }
    #endregion

    #region [ Properties - Inject ]

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public ICompanyRepository Companies { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    #endregion

    #region [ Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var role = await LocalStorage.GetItemAsStringAsync("role");
            switch (role)
            {
                case "STUDENT":
                    IsStudentViewing = true;
                    break;
                case "INSTRUCTOR":
                    IsTeacherViewing = true;
                    break;
                case "RECRUITER":
                    IsRecruiterViewing = true;
                    break;
                default:
                    break;
            }
            await this.LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Event Handlers - CommandBars - Search & Filters ]
    private void FilterChangeHandler()
    {
        OnFilterItems();
    }

    private void OnSearchHandler(ChangeEventArgs args)
    {
        var searchValue = args.Value.ToString();
        SearchText = searchValue;
        OnFilterItems();
    }

    private void OnFilterItems()
    {
        var items = this.SfMultiSelect.Value;
        var filtered = GetItemsList(items, SearchText);
        States.Items = filtered;
        StateHasChanged();
    }

    private List<JobListRowViewStates> GetItemsList(string[] options = default, string search = default)
    {
        var filtered = new List<JobListRowViewStates>();
        var result = new List<JobListRowViewStates>();

        if (options == null || options.Length <= 0)
        {
            filtered = States.OriginalItems;
        }
        else
        {
            filtered.AddRange(States.OriginalItems.Where(y => y.JobSkills.Where(z => options.Contains(z.SkillId.ToString())).Any()));
        }

        if (string.IsNullOrWhiteSpace(search))
        {
            result = filtered;
        }
        else
        {
            // have to check the string is not null first
            // for some reasons, the data can be null or blank
            result = filtered.Where(x =>
                (!string.IsNullOrEmpty(x.Title) && x.Title.Contains(search, StringComparison.InvariantCultureIgnoreCase)) ||
                (!string.IsNullOrEmpty(x.CompanyName) && x.CompanyName.Contains(search, StringComparison.InvariantCultureIgnoreCase))
            ).ToList();
        }

        return result;
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
            this.States.OriginalItems.Clear();

            this.StateHasChanged();

            var jobList = new List<Job>();
            if(IsStudentViewing)
            {
                jobList.AddRange(await Jobs.FindAll(x => x.IsAccepted && (string.IsNullOrEmpty(CompanyId) || x.CompanyId == int.Parse(CompanyId)))
                    .Include(x => x.Company)
                    .Include(x => x.JobSkills)
                    .ToListAsync());
            }
            else
            {
                jobList.AddRange(await Jobs.FindAll(x => string.IsNullOrEmpty(CompanyId) || x.CompanyId == int.Parse(CompanyId))
                    .Include(x => x.Company)
                    .Include(x => x.JobSkills)
                    .OrderBy(x => x.CompanyId)
                    .ToListAsync());
            }



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
            States.AllSkills = skills;

            this.States.Items = jobList.ToListRowList();
            this.States.Items.ForEach(x =>
            {
                x.JobSkills = jobList.FirstOrDefault(y => y.Id == x.Id)?.JobSkills.ToList();
            });

            this.States.OriginalItems = jobList.ToListRowList();
            this.States.OriginalItems.ForEach(x =>
            {
                x.JobSkills = jobList.FirstOrDefault(y => y.Id == x.Id)?.JobSkills.ToList();
            });
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
