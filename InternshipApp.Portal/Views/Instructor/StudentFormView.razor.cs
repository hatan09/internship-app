using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Wave5.UI.Forms;

namespace InternshipApp.Portal.Views;

public partial class StudentFormView
{
    #region [ Properties ]
    [Inject]
    public ILocalStorageService LocalStorage { get; private set; }

    [Inject]
    public IStudentFormRepository StudentForms { get; private set; }

    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public InstructorManager Instructors { get; set; }

    [Inject]
    public RecruiterManager Recruiters { get; private set; }
    #endregion

    #region [ Properties ]
    public bool IsStudentViewing { get; set; }
    public bool IsTeacherViewing { get; set; }
    public bool IsRecruiterViewing { get; set; }

    [Parameter]
    public StudentForm FormData { get; set; }

    public StudentForm StudentForm { get; set; } = new();

    public string Options1 { get; set; }
    public string Options2 { get; set; }
    public string Options3 { get; set; }
    public string Options4 { get; set; }
    public string Options5 { get; set; }
    public string Options6 { get; set; }
    public string Options7 { get; set; }
    public string Options8 { get; set; }
    public string Options9 { get; set; }
    public string Options10 { get; set; }
    public string Options11 { get; set; }
    public string Options12 { get; set; }
    public string Options13 { get; set; }
    public string Options14 { get; set; }
    public string Options15 { get; set; }
    #endregion

    #region [ Methods - Override ]
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
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var newForm = parameters.GetValueOrDefault<StudentForm>(nameof(this.FormData));
        var currentForm = this.FormData;

        await base.SetParametersAsync(parameters);

        if (newForm != null && newForm != currentForm)
        {
            await LoadDataAsync();
            return;
        }
    }
    #endregion

    #region [ Events ]
    private async void OnSave(EventArgs args)
    {
        await OnSaveAsync();
    }
    #endregion

    #region [ Methods - Data ]
    private async Task LoadDataAsync()
    {
        StudentForm = FormData?? new();

        Options1 = StudentForm.FulfilmentOfInternshipObjectives.ToString();
        Options2 = StudentForm.AcademicPreparationOfTheStudents.ToString();
        Options3 = StudentForm.InterestInWorkResearch.ToString();
        Options4 = StudentForm.AbilityToLearn.ToString();
        Options5 = StudentForm.Initiative.ToString();
        Options6 = StudentForm.Independence.ToString();
        Options7 = StudentForm.OrganizationPlanning.ToString();
        Options8 = StudentForm.QualityOfWorkResearch.ToString();
        Options9 = StudentForm.AnalyticalProblemSolvingSkills.ToString();
        Options10 = StudentForm.Dependability.ToString();
        Options11 = StudentForm.AcceptanceOfSuggestionsAndCriticisms.ToString();
        Options12 = StudentForm.RelationsWithOthers.ToString();
        Options13 = StudentForm.Attendance.ToString();
        Options14 = StudentForm.Punctuality.ToString();
        Options15 = StudentForm.OverallPerformance.ToString();

        StateHasChanged();
    }

    private async Task OnSaveAsync()
    {
        var form = await StudentForms.FindAll(x => x.Id == StudentForm.Id).AsTracking().FirstOrDefaultAsync();

        if(form == null)
        {
            StudentForm.IsSubmitted = IsRecruiterViewing;

            if (IsRecruiterViewing)
            {
                StudentForm.FulfilmentOfInternshipObjectives = Enum.Parse<FulfilmentOfInternshipObjectivesValues>(Options1.ToString());
                StudentForm.AcademicPreparationOfTheStudents = Enum.Parse<AcademicPreparationOfTheStudentsValues>(Options2.ToString());
                StudentForm.InterestInWorkResearch = Enum.Parse<InterestInWorkResearchValues>(Options3.ToString());
                StudentForm.AbilityToLearn = Enum.Parse<AbilityToLearnValues>(Options4.ToString());
                StudentForm.Initiative = Enum.Parse<InitiativeValues>(Options5.ToString());
                StudentForm.Independence = Enum.Parse<IndependenceValues>(Options6.ToString());
                StudentForm.OrganizationPlanning = Enum.Parse<OrganizationPlanningValues>(Options7.ToString());
                StudentForm.QualityOfWorkResearch = Enum.Parse<QualityOfWorkResearchValues>(Options8.ToString());
                StudentForm.AnalyticalProblemSolvingSkills = Enum.Parse<AnalyticalProblemSolvingSkillsValues>(Options9.ToString());
                StudentForm.Dependability = Enum.Parse<DependabilityValues>(Options10.ToString());
                StudentForm.AcceptanceOfSuggestionsAndCriticisms = Enum.Parse<AcceptanceOfSuggestionsAndCriticismsValues>(Options11.ToString());
                StudentForm.RelationsWithOthers = Enum.Parse<RelationsWithOthersValues>(Options12.ToString());
                StudentForm.Attendance = Enum.Parse<AttendanceAndPunctualityValues>(Options13.ToString());
                StudentForm.Punctuality = Enum.Parse<AttendanceAndPunctualityValues>(Options14.ToString());
                StudentForm.OverallPerformance = Enum.Parse<OverallPerformanceValues>(Options15.ToString());
            }

            StudentForms.Add(StudentForm);
            await StudentForms.SaveChangesAsync();
            return;
        }

        if (IsStudentViewing)
        {
            form.GeneralInformation = StudentForm.GeneralInformation;
            form.StudentComment = StudentForm.StudentComment;
        }
        else if (IsRecruiterViewing)
        {
            form.FulfilmentOfInternshipObjectives = Enum.Parse<FulfilmentOfInternshipObjectivesValues>(Options1.ToString());
            form.AcademicPreparationOfTheStudents = Enum.Parse<AcademicPreparationOfTheStudentsValues>(Options2.ToString());
            form.InterestInWorkResearch = Enum.Parse<InterestInWorkResearchValues>(Options3.ToString());
            form.AbilityToLearn = Enum.Parse<AbilityToLearnValues>(Options4.ToString());
            form.Initiative = Enum.Parse<InitiativeValues>(Options5.ToString());
            form.Independence = Enum.Parse<IndependenceValues>(Options6.ToString());
            form.OrganizationPlanning = Enum.Parse<OrganizationPlanningValues>(Options7.ToString());
            form.QualityOfWorkResearch = Enum.Parse<QualityOfWorkResearchValues>(Options8.ToString());
            form.AnalyticalProblemSolvingSkills = Enum.Parse<AnalyticalProblemSolvingSkillsValues>(Options9.ToString());
            form.Dependability = Enum.Parse<DependabilityValues>(Options10.ToString());
            form.AcceptanceOfSuggestionsAndCriticisms = Enum.Parse<AcceptanceOfSuggestionsAndCriticismsValues>(Options11.ToString());
            form.RelationsWithOthers = Enum.Parse<RelationsWithOthersValues>(Options12.ToString());
            form.Attendance= Enum.Parse<AttendanceAndPunctualityValues>(Options13.ToString());
            form.Punctuality = Enum.Parse<AttendanceAndPunctualityValues>(Options14.ToString());
            form.OverallPerformance = Enum.Parse<OverallPerformanceValues>(Options15.ToString());

            form.FulfilmentOfInternshipObjectivesComment = StudentForm.FulfilmentOfInternshipObjectivesComment;
            form.AcademicPreparationOfTheStudentsComment = StudentForm.AcademicPreparationOfTheStudentsComment;
            form.InterestInWorkResearchComment = StudentForm.InterestInWorkResearchComment;
            form.AbilityToLearnComment = StudentForm.AbilityToLearnComment;
            form.InitiativeComment = StudentForm.InitiativeComment;
            form.IndependenceComment = StudentForm.IndependenceComment;
            form.OrganizationPlanningComment = StudentForm.OrganizationPlanningComment;
            form.QualityOfWorkResearchComment = StudentForm.QualityOfWorkResearchComment;
            form.AnalyticalProblemSolvingSkillsComment = StudentForm.AnalyticalProblemSolvingSkillsComment;
            form.DependabilityComment = StudentForm.DependabilityComment;
            form.AcceptanceOfSuggestionsAndCriticismsComment = StudentForm.AcceptanceOfSuggestionsAndCriticismsComment;
            form.RelationsWithOthersComment = StudentForm.RelationsWithOthersComment;
            form.AttendanceAndPunctualityComment = StudentForm.AttendanceAndPunctualityComment;
            form.OverallPerformanceComment = StudentForm.OverallPerformanceComment;

            form.OverallScore = StudentForm.OverallScore;
            form.EvaluatedBy = StudentForm.EvaluatedBy;
            form.StartDate = StudentForm.StartDate;
            form.EndDate = StudentForm.EndDate;
            form.IsSubmitted = true;
        }

        StudentForms.Update(form);
        await StudentForms.SaveChangesAsync();
    }
    #endregion
}
