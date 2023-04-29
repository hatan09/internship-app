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
    public bool IsEditable { get; set; } = false;

    [Parameter]
    public EventCallback OnEditButtonClicked { get; set; }

    #region [ Properties - Inject ]
    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }
    #endregion

    #region [ Properties ]
    public List<StudentSkill> StudentSkills { get; set; }
    public List<JobSkill> JobSkills { get; set; }
    public List<Skill> AllSkills { get; set; }

    public bool IsStudentSkills { get; set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            StudentSkills = new();
            JobSkills = new();
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
            
            if(IsStudent && IsJob == true || !IsStudent && !IsJob == true)
            {
                return;
            }
            else if (IsStudent)
            {
                IsStudentSkills = true;

                var student = await Students.FindAll(x => x.Id == StudentId).Include(x => x.StudentSkills).FirstOrDefaultAsync();
                StudentSkills = student.StudentSkills.ToList();
                var skills = await Skills.FindAll(x => student.StudentSkills.Select(x => x.SkillId).Contains(x.Id)).ToListAsync();
                AllSkills = skills;
            }
            else if (IsJob)
            {
                IsStudentSkills = false;

                var job = await Jobs.FindAll(x => x.Id == (int) JobId).Include(x => x.JobSkills).FirstOrDefaultAsync();
                JobSkills = job.JobSkills.ToList();
                var skills = await Skills.FindAll(x => job.JobSkills.Select(x => x.SkillId).Contains(x.Id)).ToListAsync();
                AllSkills = skills;
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
    #endregion
}
