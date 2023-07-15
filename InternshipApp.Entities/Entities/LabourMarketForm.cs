namespace InternshipApp.Core.Entities;

public class LabourMarketForm : BaseEntity<int>
{
    public string StudentId { get; set; }

    public string ContactName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

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
    public string Prepared { get; set; }
    public string Improvements { get; set; }
    public string Skills { get; set; }

    public bool IsSubmitted { get; set; } = false;
}
