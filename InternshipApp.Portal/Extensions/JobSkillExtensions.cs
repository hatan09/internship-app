using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI;


namespace InternshipApp.Portal.Views;

public static class JobSkillExtensions
{
    #region [ Public Methods - ListRow ]
    public static List<JobSkillListRowViewStates> ToListRowList(this List<JobSkill> list)
    {
        var result = new List<JobSkillListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static JobSkillListRowViewStates ToListRow(this JobSkill entity)
    {

        var viewStates = ToViewStates<JobSkillListRowViewStates>(entity);

        return viewStates;
    }

    public static JobSkill ToEntity(this JobSkillListRowViewStates viewStates)
    {
        return ToEntity<JobSkill>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static JobSkillFormViewStates ToFormViewStates(this JobSkill entity)
    {

        var viewmodel = ToViewStates<JobSkillFormViewStates>(entity);

        return viewmodel;
    }

    public static JobSkill ToEntity(this JobSkillFormViewStates viewStates)
    {
        return ToEntity<JobSkill>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    //public static JobSkillDetailsViewStates ToDetailsViewStates(this JobSkill entity)
    //{

    //    var viewmodel = ToViewStates<JobSkillDetailsViewStates>(entity);

    //    return viewmodel;
    //}

    //public static JobSkill ToEntity(this JobSkillDetailsViewStates viewStates)
    //{
    //    return ToEntity<JobSkill>(viewStates);
    //}
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this JobSkill entity)

        where TBaseViewModel : BaseJobSkillViewStates, new()
    {
        return new TBaseViewModel()
        {
            JobId = entity.JobId,
            SkillId = entity.SkillId,
            Level = entity.Level.ToString(),
            Weight = entity.Weight,
            WeightText = entity.Weight.ToString(),
            Description = entity.Description,
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseJobSkillViewStates viewModel)
        where TEntity : JobSkill, new()
    {
        return new TEntity()
        {
            JobId = viewModel.JobId,
            SkillId = viewModel.SkillId,
            Level = Enum.Parse<SkillLevel>(viewModel.Level),
            Weight = viewModel.Weight,
            Description = viewModel.Description,
        };
    }
    #endregion

    #region [ Public Methods - Options ]
    // Skills
    public static List<Option<string>> SkillOptions(this JobSkillFormViewStates states)
    {
        return states.Skills.ToOptionList(x => x.Id.ToString(), x => x.Name, null);
    }

    public static string GetSelectedSkillId(this JobSkillFormViewStates states)
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = states.SkillOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(states.SelectedSkill, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }

    // Level
    public static List<Option<string>> LevelOptions(this JobSkillFormViewStates states)
    {
        return states.Levels.ToOptionList(x => x, x => x, null);
    }

    public static string GetSelectedLevelName(this JobSkillFormViewStates states)
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = states.LevelOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(states.SelectedLevel, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }
    #endregion
}
