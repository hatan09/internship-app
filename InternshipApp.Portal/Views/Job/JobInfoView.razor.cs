using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class JobInfoView
{
    #region [ Fields ]

    #endregion

    #region [ Properties - Parameter ]
    [Parameter]
    public string JobId { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public ISkillRepository Skills { get; set; }

    [Inject]
    public IJobRepository Jobs { get; set; }

    [Inject]
    public IMatchingService MatchingService { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }
    #endregion

    #region [ Properties - Panel ]
    protected FormRequest<FormAction, Job> JobFormRequest { get; private set; }
    #endregion

    #region [ Properties - Data ]
    protected JobDetailsViewStates States { get; private set; }
    #endregion

    #region [ Protected Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        States = new();
        await base.OnInitializedAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var currentJobId = this.JobId;
        var parameterJobId = parameters.GetValueOrDefault<string>(nameof(this.JobId));

        await base.SetParametersAsync(parameters);

        if (currentJobId != parameterJobId)
        {
            await this.LoadDataAsync();
        }
    }
    #endregion

    #region [ Private Methods - Data ]
    private async void OnApply()
    {
        var student = await GetStudentAsync();
        if(student.StudentJobs.FirstOrDefault(x => x.JobId == int.Parse(JobId)) != null)
        {
            return;
        }
        var studentJob = new StudentJob()
        {
            StudentId = student.Id.ToString(),
            JobId = int.Parse(JobId)
        };

        await OnUpdateApplicationAsync(studentJob);
        await LoadDataAsync();

    }

    private async Task<Student> GetStudentAsync()
    {
        var user = await LocalStorage.GetItemAsync<User>("login-user-info");
        string studentId = string.Empty;
        if(user != null)
        {
            studentId = user.Id;
        }
        var student = await Students.FindAll(x => x.Id == studentId)
            .AsNoTracking()
            .Include(x => x.StudentSkills)
            .FirstOrDefaultAsync();

        return student;
    }

    private async Task OnUpdateApplicationAsync(StudentJob studentJob)
    {
        var job = await Jobs.FindAll(x => x.Id == int.Parse(JobId)).Include(x => x.StudentJobs).FirstOrDefaultAsync();
        job?.StudentJobs.Add(studentJob);
        Jobs.Update(job);
        await Jobs.SaveChangesAsync();
    }

    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(this.JobId, nameof(this.JobId));

        try
        {
            var student = await GetStudentAsync();

            var item = await this.Jobs.FindAll(x => x.Id == int.Parse(JobId))
                .Include(x => x.Company)
                .Include(x => x.JobSkills)
                .Include(x => x.StudentJobs.Where(y => y.StudentId == student.Id))
                .FirstOrDefaultAsync();

            if (item is null)
            {
                this.States = null;
                return;
            }

            this.States = item.ToDetailsViewStates();
            States.HasApplied = item.StudentJobs.Any();
            States.JobSkills = item.JobSkills.ToList();
            var skillIds = item.JobSkills.Select(x => x.SkillId).ToList();

            var company = item.Company;
            if(company != null)
            {
                States.CompanyName = company.Title;
                States.Address = company.Address;
            }

            var matching = MatchingService.GetMatchingPoint(student.StudentSkills?.ToList(), item.JobSkills?.ToList());
            States.Matching = matching;

            var skills = await Skills.FindAll(x => skillIds.Contains(x.Id)).AsNoTracking().ToListAsync();
            States.Skills.AddRange(skills);
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
