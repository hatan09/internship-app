using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RCode;

namespace InternshipApp.Portal.Views;

public partial class FinalSubmissionView
{
    #region [ Properties ]
    [Parameter]
    public string StudentId { get; set; }

    public StudentForm StudentForm { get; set; }
    public LabourMarketForm LabourMarketForm { get; set; }
    #endregion

    #region [ Properties - Inject ]
    [Inject]
    public RecruiterManager Recruiters { get; set; }

    [Inject]
    public StudentManager Students { get; set; }

    [Inject]
    public IStudentFormRepository StudentForms { get; set; }

    [Inject]
    public ILabourMarketFormRepository LabourMarketForms { get; set; }

    [Inject]
    public IEvaluationRepository Evaluations { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; private set; }
    #endregion

    #region [ Methods - Override ]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadDataAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region [ Methods - Data ]
    private async Task LoadDataAsync()
    {
        Guard.ParamIsNullOrEmpty(StudentId, nameof(StudentId));

        var student = await Students
                .FindAll(x => x.Id == StudentId)
                .Include(x => x.StudentJobs.Where(x => x.Status == ApplyStatus.HIRED))
                .ThenInclude(x => x.Job)
                .ThenInclude(x => x.Company)
                .FirstOrDefaultAsync();

        StudentForm = await StudentForms.FindAll(x => x.StudentId == StudentId).FirstOrDefaultAsync();
        if (StudentForm == null)
        {
            var evaluation = await Evaluations.FindByStudentAsync(StudentId);
            var score = 0;
            evaluation.ForEach(x =>
            {
                score += x.Score;
            });
            score /= evaluation.Count;

            StudentForm = new()
            {
                StudentId = StudentId,
                StudentName = student?.FullName,
                CompanyName = student?.StudentJobs?.FirstOrDefault(x => x.Status == ApplyStatus.HIRED)?.Job?.Company?.Title,
                AverageReportScore = score
            };
        }

        LabourMarketForm = await LabourMarketForms.FindAll(x => x.StudentId == StudentId).FirstOrDefaultAsync();
        LabourMarketForm ??= new()
        {
            StudentId = StudentId
        };

        StateHasChanged();
    }
    #endregion
}
