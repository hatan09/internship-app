namespace InternshipApp.Core.Entities;

public class LabourMarketForm : BaseEntity<int>
{
    public string? StudentId { get; set; } = string.Empty;

    public string? ContactName { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;

    public int TheEducationalGoalsAndProgramOutcomesAreClearAndAppropriateAsStated { get; set; }
    public int TheCurriculumSatisfactorilyCoversTheProfessionOfComputingInTermsOfBreadthAndDepthOfKnowledge { get; set; }
    public int AnAbilityToApplyProfessionalKnowledgeInPractice { get; set; }
    public int AnAbilityToPlanOrganizeAndManageAProject { get; set; }
    public int AnAbilityInProblemSolving { get; set; }
    public int InitiativeTaking { get; set; }
    public int AnAbilityToFunctionEffectivelyOnTeams { get; set; }
    public int AnAbilityToCommunicateEffectively { get; set; }
    public int AnAbilityToEngageInContinuingProfessionalDevelopment { get; set; }
    public int Independence { get; set; }
    public int RelationsWithOthers { get; set; }

    public int IUGraduates { get; set; }
    public string? Prepared { get; set; } = string.Empty;
    public string? Improvements { get; set; } = string.Empty;
    public string? Skills { get; set; } = string.Empty;

    public bool IsSubmitted { get; set; } = false;
}
