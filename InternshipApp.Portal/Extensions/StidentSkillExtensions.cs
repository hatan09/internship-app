using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI;


namespace InternshipApp.Portal.Views;

public static class StudentSkillExtensions
{
    #region [ Public Methods - ListRow ]
    public static List<StudentSkillListRowViewStates> ToListRowList(this List<StudentSkill> list)
    {
        var result = new List<StudentSkillListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static StudentSkillListRowViewStates ToListRow(this StudentSkill entity)
    {

        var viewStates = ToViewStates<StudentSkillListRowViewStates>(entity);

        return viewStates;
    }

    public static StudentSkill ToEntity(this StudentSkillListRowViewStates viewStates)
    {
        return ToEntity<StudentSkill>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    //public static StudentSkillFormViewStates ToFormViewStates(this StudentSkill entity)
    //{

    //    var viewmodel = ToViewStates<StudentSkillFormViewStates>(entity);

    //    return viewmodel;
    //}

    //public static StudentSkill ToEntity(this StudentSkillFormViewStates viewStates)
    //{
    //    return ToEntity<StudentSkill>(viewStates);
    //}
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    //public static StudentSkillDetailsViewStates ToDetailsViewStates(this StudentSkill entity)
    //{

    //    var viewmodel = ToViewStates<StudentSkillDetailsViewStates>(entity);

    //    return viewmodel;
    //}

    //public static StudentSkill ToEntity(this StudentSkillDetailsViewStates viewStates)
    //{
    //    return ToEntity<StudentSkill>(viewStates);
    //}
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this StudentSkill entity)

        where TBaseViewModel : BaseStudentSkillViewStates, new()
    {
        return new TBaseViewModel()
        {
            StudentId = entity.StudentId,
            SkillId = (int) entity.SkillId,
            Level = entity.Level.ToString(),
            Description = entity.Description,
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseStudentSkillViewStates viewModel)
        where TEntity : StudentSkill, new()
    {
        return new TEntity()
        {
            StudentId = viewModel.StudentId,
            SkillId = viewModel.SkillId,
            Level = Enum.Parse<SkillLevel>(viewModel.Level),
            Description = viewModel.Description,
        };
    }
    #endregion

    #region [ Public Methods - Options ]
    // Skills
    public static List<Option<string>> SkillOptions(this StudentSkillListRowViewStates states)
    {
        return states.Skills.ToOptionList(x => x.Id.ToString(), x => x.Name, null);
    }

    public static string GetSelectedSkillId(this StudentSkillListRowViewStates states)
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
    public static List<Option<string>> LevelOptions(this StudentSkillListRowViewStates states)
    {
        return states.Levels.ToOptionList(x => x, x => x, null);
    }

    public static string GetSelectedLevelName(this StudentSkillListRowViewStates states)
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
