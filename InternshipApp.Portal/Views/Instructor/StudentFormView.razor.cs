using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

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
            StateHasChanged();
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
            LoadData();
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
    private void LoadData()
    {
        StudentForm = FormData ?? new();

        Options1 = ((int)StudentForm.FulfilmentOfInternshipObjectives).ToString();
        Options2 = ((int)StudentForm.AcademicPreparationOfTheStudents).ToString();
        Options3 = ((int)StudentForm.InterestInWorkResearch).ToString();
        Options4 = ((int)StudentForm.AbilityToLearn).ToString();
        Options5 = ((int)StudentForm.Initiative).ToString();
        Options6 = ((int)StudentForm.Independence).ToString();
        Options7 = ((int)StudentForm.OrganizationPlanning).ToString();
        Options8 = ((int)StudentForm.QualityOfWorkResearch).ToString();
        Options9 = ((int)StudentForm.AnalyticalProblemSolvingSkills).ToString();
        Options10 = ((int)StudentForm.Dependability).ToString();
        Options11 = ((int)StudentForm.AcceptanceOfSuggestionsAndCriticisms).ToString();
        Options12 = ((int)StudentForm.RelationsWithOthers).ToString();
        Options13 = ((int)StudentForm.Attendance).ToString();
        Options14 = ((int)StudentForm.Punctuality).ToString();
        Options15 = ((int)StudentForm.OverallPerformance).ToString();

        StateHasChanged();
    }

    private async Task OnSaveAsync()
    {
        var form = await StudentForms.FindAll(x => x.Id == StudentForm.Id).AsTracking().FirstOrDefaultAsync();

        if (form == null)
        {
            StudentForm.IsSubmitted = IsRecruiterViewing;

            if (IsRecruiterViewing)
            {
                StudentForm.FulfilmentOfInternshipObjectives = (FulfilmentOfInternshipObjectivesValues)int.Parse(Options1);
                StudentForm.AcademicPreparationOfTheStudents = (AcademicPreparationOfTheStudentsValues)int.Parse(Options2);
                StudentForm.InterestInWorkResearch = (InterestInWorkResearchValues)int.Parse(Options3);
                StudentForm.AbilityToLearn = (AbilityToLearnValues)int.Parse(Options4);
                StudentForm.Initiative = (InitiativeValues)int.Parse(Options5);
                StudentForm.Independence = (IndependenceValues)int.Parse(Options6);
                StudentForm.OrganizationPlanning = (OrganizationPlanningValues)int.Parse(Options7);
                StudentForm.QualityOfWorkResearch = (QualityOfWorkResearchValues)int.Parse(Options8);
                StudentForm.AnalyticalProblemSolvingSkills = (AnalyticalProblemSolvingSkillsValues)int.Parse(Options9);
                StudentForm.Dependability = (DependabilityValues)int.Parse(Options10);
                StudentForm.AcceptanceOfSuggestionsAndCriticisms = (AcceptanceOfSuggestionsAndCriticismsValues)int.Parse(Options11);
                StudentForm.RelationsWithOthers = (RelationsWithOthersValues)int.Parse(Options12);
                StudentForm.Attendance = (AttendanceAndPunctualityValues)int.Parse(Options13);
                StudentForm.Punctuality = (AttendanceAndPunctualityValues)int.Parse(Options14);
                StudentForm.OverallPerformance = (OverallPerformanceValues)int.Parse(Options15);
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
            form.FulfilmentOfInternshipObjectives = (FulfilmentOfInternshipObjectivesValues)int.Parse(Options1);
            form.AcademicPreparationOfTheStudents = (AcademicPreparationOfTheStudentsValues)int.Parse(Options2);
            form.InterestInWorkResearch = (InterestInWorkResearchValues)int.Parse(Options3);
            form.AbilityToLearn = (AbilityToLearnValues)int.Parse(Options4);
            form.Initiative = (InitiativeValues)int.Parse(Options5);
            form.Independence = (IndependenceValues)int.Parse(Options6);
            form.OrganizationPlanning = (OrganizationPlanningValues)int.Parse(Options7);
            form.QualityOfWorkResearch = (QualityOfWorkResearchValues)int.Parse(Options8);
            form.AnalyticalProblemSolvingSkills = (AnalyticalProblemSolvingSkillsValues)int.Parse(Options9);
            form.Dependability = (DependabilityValues)int.Parse(Options10);
            form.AcceptanceOfSuggestionsAndCriticisms = (AcceptanceOfSuggestionsAndCriticismsValues)int.Parse(Options11);
            form.RelationsWithOthers = (RelationsWithOthersValues)int.Parse(Options12);
            form.Attendance = (AttendanceAndPunctualityValues)int.Parse(Options13);
            form.Punctuality = (AttendanceAndPunctualityValues)int.Parse(Options14);
            form.OverallPerformance = (OverallPerformanceValues)int.Parse(Options15);

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
