using Blazored.LocalStorage;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Portal.Views;

public partial class LabourMarketFormView
{
    #region [ Properties ]
    [Inject]
    public ILocalStorageService LocalStorage { get; private set; }

    [Inject]
    public ILabourMarketFormRepository LabourMarketForms { get; private set; }
    #endregion

    #region [ Properties ]
    public bool IsRecruiterViewing { get; set; }

    [Parameter]
    public LabourMarketForm FormData { get; set; }

    public LabourMarketForm LabourMarketForm { get; set; } = new();

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
    #endregion

    #region [ Methods - Override ]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var role = await LocalStorage.GetItemAsStringAsync("role");
            switch (role)
            {
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
        var newForm = parameters.GetValueOrDefault<LabourMarketForm>(nameof(this.FormData));
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
        LabourMarketForm = FormData ?? new();

        Options1 = LabourMarketForm.TheEducationalGoalsAndProgramOutcomesAreClearAndAppropriateAsStated.ToString();
        Options2 = LabourMarketForm.TheCurriculumSatisfactorilyCoversTheProfessionOfComputingInTermsOfBreadthAndDepthOfKnowledge.ToString();
        Options3 = LabourMarketForm.AnAbilityToApplyProfessionalKnowledgeInPractice.ToString();
        Options4 = LabourMarketForm.AnAbilityToPlanOrganizeAndManageAProject.ToString();
        Options5 = LabourMarketForm.AnAbilityInProblemSolving.ToString();
        Options6 = LabourMarketForm.InitiativeTaking.ToString();
        Options7 = LabourMarketForm.AnAbilityToFunctionEffectivelyOnTeams.ToString();
        Options8 = LabourMarketForm.ProfessionalEthicsAndResponsibility.ToString();
        Options9 = LabourMarketForm.AnAbilityToCommunicateEffectively.ToString();
        Options10 = LabourMarketForm.AnAbilityToEngageInContinuingProfessionalDevelopment.ToString();
        Options11 = LabourMarketForm.Independence.ToString();
        Options12 = LabourMarketForm.RelationsWithOthers.ToString();

        StateHasChanged();
    }

    private async Task OnSaveAsync()
    {
        var form = await LabourMarketForms.FindAll(x => x.Id == LabourMarketForm.Id).AsTracking().FirstOrDefaultAsync();

        if (form == null)
        {
            LabourMarketForm.IsSubmitted = IsRecruiterViewing;

            if (IsRecruiterViewing)
            {
                LabourMarketForm.TheEducationalGoalsAndProgramOutcomesAreClearAndAppropriateAsStated = int.Parse(Options1);
                LabourMarketForm.TheCurriculumSatisfactorilyCoversTheProfessionOfComputingInTermsOfBreadthAndDepthOfKnowledge = int.Parse(Options2);
                LabourMarketForm.AnAbilityToApplyProfessionalKnowledgeInPractice = int.Parse(Options3);
                LabourMarketForm.AnAbilityToPlanOrganizeAndManageAProject = int.Parse(Options4);
                LabourMarketForm.AnAbilityInProblemSolving = int.Parse(Options5);
                LabourMarketForm.InitiativeTaking = int.Parse(Options6);
                LabourMarketForm.AnAbilityToFunctionEffectivelyOnTeams = int.Parse(Options7);
                LabourMarketForm.ProfessionalEthicsAndResponsibility = int.Parse(Options8);
                LabourMarketForm.AnAbilityToCommunicateEffectively = int.Parse(Options9);
                LabourMarketForm.AnAbilityToEngageInContinuingProfessionalDevelopment = int.Parse(Options10);
                LabourMarketForm.Independence = int.Parse(Options11);
                LabourMarketForm.RelationsWithOthers = int.Parse(Options12);
            }

            LabourMarketForms.Add(LabourMarketForm);
            await LabourMarketForms.SaveChangesAsync();
            return;
        }

        if (IsRecruiterViewing)
        {
            form.TheEducationalGoalsAndProgramOutcomesAreClearAndAppropriateAsStated = int.Parse(Options1);
            form.TheCurriculumSatisfactorilyCoversTheProfessionOfComputingInTermsOfBreadthAndDepthOfKnowledge = int.Parse(Options2);
            form.AnAbilityToApplyProfessionalKnowledgeInPractice = int.Parse(Options3);
            form.AnAbilityToPlanOrganizeAndManageAProject = int.Parse(Options4);
            form.AnAbilityInProblemSolving = int.Parse(Options5);
            form.InitiativeTaking = int.Parse(Options6);
            form.AnAbilityToFunctionEffectivelyOnTeams = int.Parse(Options7);
            form.ProfessionalEthicsAndResponsibility = int.Parse(Options8);
            form.AnAbilityToCommunicateEffectively = int.Parse(Options9);
            form.AnAbilityToEngageInContinuingProfessionalDevelopment = int.Parse(Options10);
            form.Independence = int.Parse(Options11);
            form.RelationsWithOthers = int.Parse(Options12);

            form.ContactName = LabourMarketForm.ContactName;
            form.Phone = LabourMarketForm.Phone;
            form.Email = LabourMarketForm.Email;
            form.IUGraduates = LabourMarketForm.IUGraduates;
            form.Prepared = LabourMarketForm.Prepared;
            form.Improvements = LabourMarketForm.Improvements;
            form.Skills = LabourMarketForm.Skills;
            form.IsSubmitted = true;
        }

        LabourMarketForms.Update(form);
        await LabourMarketForms.SaveChangesAsync();
    }
    #endregion
}
