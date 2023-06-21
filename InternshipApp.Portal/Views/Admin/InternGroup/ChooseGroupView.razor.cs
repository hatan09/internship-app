using System.Collections.ObjectModel;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;
using RCode.UI.ViewModels;

namespace InternshipApp.Portal.Views;

public partial class ChooseGroupView
{
    #region [ Properties ]
    private ObservableCollection<InternGroup> Groups { get; set; }
    private string SelectedGroupName { get; set; }
    private string NoStudents { get; set; }
    #endregion

    #region [ Properties - Param ]
    [Parameter]
    public List<string> StudentIds { get; set; }

    [Parameter]
    public EventCallback UpdateCallBack { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IInternGroupRepository InternGroups { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        Groups = new();
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            await LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Events Handlers ]
    public async void OnAddButtonClicked()
    {
        var result = int.TryParse(GetSelectedGroupId(), out var groupId);
        if(result && groupId > 0)
        {
            await AddToGroup(groupId);
            await JSRuntime.InvokeVoidAsync("alert", $"{NoStudents} students have been added to {groupId}.");
            await UpdateCallBack.InvokeAsync();
        }
        else
        {
            await JSRuntime.InvokeVoidAsync("alert", "No group was selected.");
        }
    }
    #endregion

    #region [ Methods - Data ]
    public async Task AddToGroup(int groupId)
    {
        var group = await InternGroups.FindAll(x => x.Id == groupId).Include(x => x.Students).AsTracking().FirstOrDefaultAsync();
        if(group == null)
        {
            await JSRuntime.InvokeVoidAsync("alert", "No group was found.");
            return;
        }

        var students = new List<Student>();
        foreach(var id in StudentIds)
        {
            var student = await Students.FindAll(x => x.Id == id).AsTracking().FirstOrDefaultAsync();
            if(student != null)
            {
                students.Add(student);
            }
        }

        foreach (var student in students)
        {
            group.Students.Add(student);
        }
        InternGroups.Update(group);
        await InternGroups.SaveChangesAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            NoStudents = StudentIds == null ? "0" : StudentIds.Count.ToString();
            var allGroups = await InternGroups.FindAll().Include(x => x.Students).OrderBy(x => x.Students.Count).AsNoTracking().ToListAsync();
            Groups = allGroups.ToObservableCollection();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            StateHasChanged();

        }
    }
    #endregion

    #region [ Methods - Options ]
    public List<Option<string>> GroupOptions()
    {
        return Groups.ToOptionList(x => x.Id.ToString(), x => $"{x.Title} ({x.Students.Count})", null);
    }

    public string GetSelectedGroupId()
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = GroupOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(SelectedGroupName, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }
    #endregion
}
