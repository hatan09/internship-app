namespace InternshipApp.Core.Entities;

public class StudentForm : BaseEntity<int>
{
    public string? StudentId { get; set; } = string.Empty;
    public string? GeneralInformation { get; set; } = string.Empty;
    public string? StudentName { get; set; } = string.Empty;
    public string? CompanyName { get; set; } = string.Empty;
    public string? EvaluatedBy { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AverageReportScore { get; set; }
    public int OverallScore { get; set; }
    public string? StudentComment { get; set; } = string.Empty;
    public bool IsSubmitted { get; set; }

    #region [ Properties - Form Evaluations ]
    public FulfilmentOfInternshipObjectivesValues FulfilmentOfInternshipObjectives { get; set; }
    public string? FulfilmentOfInternshipObjectivesComment { get; set; } = string.Empty;

    public AcademicPreparationOfTheStudentsValues AcademicPreparationOfTheStudents { get; set; }
    public string? AcademicPreparationOfTheStudentsComment { get; set; } = string.Empty;

    public InterestInWorkResearchValues InterestInWorkResearch { get; set; }
    public string? InterestInWorkResearchComment { get; set; } = string.Empty;

    public AbilityToLearnValues AbilityToLearn { get; set; }
    public string? AbilityToLearnComment { get; set; } = string.Empty;

    public InitiativeValues Initiative { get; set; }
    public string? InitiativeComment { get; set; } = string.Empty;

    public IndependenceValues Independence { get; set; }
    public string? IndependenceComment { get; set; } = string.Empty;

    public OrganizationPlanningValues OrganizationPlanning { get; set; }
    public string? OrganizationPlanningComment { get; set; } = string.Empty;

    public QualityOfWorkResearchValues QualityOfWorkResearch { get; set; }
    public string? QualityOfWorkResearchComment { get; set; } = string.Empty;

    public AnalyticalProblemSolvingSkillsValues AnalyticalProblemSolvingSkills { get; set; }
    public string? AnalyticalProblemSolvingSkillsComment { get; set; } = string.Empty;

    public DependabilityValues Dependability { get; set; }
    public string? DependabilityComment { get; set; } = string.Empty;

    public AcceptanceOfSuggestionsAndCriticismsValues AcceptanceOfSuggestionsAndCriticisms { get; set; }
    public string? AcceptanceOfSuggestionsAndCriticismsComment { get; set; } = string.Empty;

    public RelationsWithOthersValues RelationsWithOthers { get; set; }
    public string? RelationsWithOthersComment { get; set; } = string.Empty;

    public AttendanceAndPunctualityValues Attendance { get; set; }
    public AttendanceAndPunctualityValues Punctuality { get; set; }
    public string? AttendanceAndPunctualityComment { get; set; } = string.Empty;

    public OverallPerformanceValues OverallPerformance { get; set; }
    public string? OverallPerformanceComment { get; set; } = string.Empty;
    #endregion

}

#region [ Enums ]
public enum FulfilmentOfInternshipObjectivesValues
{
    Objectives_not_met,
    Few_objectives_fulfilled,
    Most_objectives_fulfilled,
    All_objectives_adequately_objectives,
}

public enum AcademicPreparationOfTheStudentsValues
{
    Below_average,
    Average,
    Good,
    Very_good,
    Excellent,
}

public enum InterestInWorkResearchValues
{
    Little_interest_or_enthusiasm,
    Sometimes_interested,
    Generally_interested,
    Very_enthusiastic,
}

public enum AbilityToLearnValues
{
    Requires_more_time_to_learn_new_concepts_or_takes_even_with_mentorship,
    Can_learn_with_appropriate_mentorship_and_guidance_within_a_reasonable_time_frame,
    Generally_capable_of_learning_new_concepts_or_tasks_within_a_reasonable_time_frame,
    Exceptional_ability_to_learn_new_concepts_or_tasks_very_quickly,
}

public enum InitiativeValues
{
    Always_waits_to_be_told_what_to_do_next,
    Acts_voluntarily_only_in_routine_time_matters,
    Demonstrates_some_initiative_on_selected_challenges,
    Consistent_selfstarter_and_always_strives_to_add_value,
}

public enum IndependenceValues
{
    Requires_constant_mentorship_or_instruction,
    Works_well_under_adequate_supervision,
    Can_usually_work_independently,
    Requires_minimal_supervision_very_independent,
}

public enum OrganizationPlanningValues
{
    Generally_not_organized_and_ill_prepared_for_the_tasks_at_hand,
    Occasionally_not_organized_and_unprepared_for_the_tasks_at_hand,
    Generally_organized_and_usually_plans_ahead,
    Very_organized_and_always_equipped_with_a_plan_of_action,
}

public enum QualityOfWorkResearchValues
{
    Work_usually_completed_in_a_careless_manner_and_constantly_plagued_with,
    Work_usually_required_review__satisfactory_work__but_may_contain_some_errors,
    Usually_thorough___Generally__good_work_with_very_few_errors,
    Very_thorough___Outstanding_attention_to_detail,
}

public enum AnalyticalProblemSolvingSkillsValues
{
    Usually_poorly_understanding_of_the_problems_at_hand__experiences_difficulty_in_evaluating_and_selecting_among_alternatives__consistently_gets_stuck_in_the_problem_solving_process,
    Sometimes_understands_the_problems_at_hand__analytical_skills_are_satisfactory_and_usually_require_assistance_in_solving_problems,
    Generally_understands_the_problems_at_hand__good_analytical_skills_but_may_require_assistance_in_solving_problems,
    Always_understands_the_problems_at_hand__regularly_exercises_critical_thinking_and_systematic_problem_solving___Carefully_evaluates_and_selects_among_alternatives_and_successfully_solvers_problems_with_little_assistance,
}

public enum DependabilityValues
{
    Unreliable,
    Sometimes_neglectful_or_careless,
    Usually_dependable,
    Completely_dependable__worry_free,
}

public enum AcceptanceOfSuggestionsAndCriticismsValues
{
    Resents_suggestions_and_criticisms_by_supervisor___No_demonstrated_effort_to_improve,
    Reluctantly_accepts_suggestions_and_criticisms_by_supervisor___Very_little_demonstrated_effort_to_improve,
    Takes_prompt_action_in_response_to_suggestions_and_feedback_from_supervisor,
    Accepts_suggestions_and_criticisms_by_supervisor_and_usually_implements_corrective_behavior_over_time,
}

public enum RelationsWithOthersValues
{
    General_difficulty_working_with_others,
    Works_with_other_satisfactorily,
    Works_very_well_with_others,
    Has_difficulty_with_some_individuals,
}

public enum AttendanceAndPunctualityValues
{
    Poor,
    Fair,
    Satisfactory,
    Good,
    Excellent
}

public enum OverallPerformanceValues
{
    Marginal,
    Average,
    Good,
    Very_Good,
    Outstanding,
}
#endregion