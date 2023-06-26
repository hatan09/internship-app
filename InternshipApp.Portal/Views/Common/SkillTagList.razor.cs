using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class SkillTagList
{
    [Parameter]
    public string? StudentId { get; set; }

    [Parameter]
    public int? JobId { get; set; }

    [Parameter]
    public List<JobSkill>? JobSkills { get; set; }

    [Parameter]
    public List<StudentSkill>? StudentSkills { get; set; }

    [Parameter]
    public List<Skill>? AllSkills { get; set; }

    [Parameter]
    public bool IsEditable { get; set; } = false;

    [Parameter]
    public EventCallback OnEditButtonClickedCallback { get; set; }

    #region [ Properties ]
    public bool IsStudentSkills { get; set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            StudentSkills = new();
            JobSkills = new();
            AllSkills = new();
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {

        }
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newJobSkills = parameters.GetValueOrDefault<List<JobSkill>>(nameof(this.JobSkills));
        var currentJobSkills = this.JobSkills;

        var newStudentSkills = parameters.GetValueOrDefault<List<StudentSkill>>(nameof(this.StudentSkills));
        var currentStudentSkills = this.StudentSkills;

        var newAllSkills = parameters.GetValueOrDefault<List<Skill>>(nameof(this.AllSkills));
        var currentAllSkills = this.AllSkills;

        await base.SetParametersAsync(parameters);

        if ((newJobSkills != null && newJobSkills != currentJobSkills) || 
            (newStudentSkills != null && newStudentSkills != currentStudentSkills) ||
            (newAllSkills != null && newAllSkills != currentAllSkills))
        {
            JobSkills = newJobSkills;
            AllSkills = newAllSkills;
            await this.LoadDataAsync();
            return;
        }
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
            var IsStudent = !string.IsNullOrEmpty(StudentId);
            var IsJob = JobId > 0;
            
            if((IsStudent && IsJob) || (!IsStudent && !IsJob))
            {
                return;
            }
            else if (IsStudent)
            {
                IsStudentSkills = true;
            }
            else if (IsJob)
            {
                IsStudentSkills = false;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            this.StateHasChanged();
        }
    }

    public async void OnEdit()
    {
        await OnEditButtonClickedCallback.InvokeAsync();
    }
    #endregion
}
