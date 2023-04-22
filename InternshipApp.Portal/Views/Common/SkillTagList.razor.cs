using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class SkillTagList
{
    [Parameter]
    public string? StudentId { get; set; }

    [Parameter]
    public int? JobId { get; set; }

    [Parameter]
    public List<int> SkillIds { get; set; }

    #region [ Properties - Inject ]
    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {


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

    #endregion

    #region [ Event Handlers - DataList ]
    #endregion



    #region [ Event Handlers - CommandBars - Buttons ]

    private void OnAddButtonClicked(EventArgs e)
    {
        this.StateHasChanged();
    }
    #endregion

    #region [ Event Handlers - Panel ]
    protected async Task OnFormResultReceived(FormResult<Student> result)
    {
        var tasks = new List<Task>
        {
            this.LoadDataAsync()
        };

        await Task.WhenAll(tasks);
    }
    #endregion

    #region [ Private Methods - Data ]
    private async Task LoadDataAsync()
    {
        try
        {

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
