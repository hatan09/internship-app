using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI;

namespace InternshipApp.Portal.Views;

public static class SkillExtension
{
    #region [ Public Methods - ListRow ]
    public static List<SkillListRowViewStates> ToListRowList(this List<Skill> list)
    {
        var result = new List<SkillListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static SkillListRowViewStates ToListRow(this Skill entity)
    {

        var viewStates = ToViewStates<SkillListRowViewStates>(entity);

        return viewStates;
    }

    public static Skill ToEntity(this SkillListRowViewStates viewStates)
    {
        return ToEntity<Skill>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static SkillFormViewStates ToFormViewStates(this Skill entity)
    {

        var viewmodel = ToViewStates<SkillFormViewStates>(entity);

        return viewmodel;
    }

    public static Skill ToEntity(this SkillFormViewStates viewStates)
    {
        return ToEntity<Skill>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static SkillDetailsViewStates ToDetailsViewStates(this Skill entity)
    {
        var viewmodel = ToViewStates<SkillDetailsViewStates>(entity);

        return viewmodel;
    }

    public static Skill ToEntity(this SkillDetailsViewStates viewStates)
    {
        return ToEntity<Skill>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Skill entity)

        where TBaseViewModel : BaseSkillViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

            Name = entity.Name,
            SkillType = entity.Type.ToString(),
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseSkillViewStates viewModel)
        where TEntity : Skill, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

            Name = viewModel.Name,
            Type = Enum.Parse<SkillType>(viewModel.SkillType)
        };
    }
    #endregion

    #region [ Methods - Options ]
    // Type
    public static List<Option<string>> TypeOptions(this SkillFormViewStates states)
    {
        return states.Types.ToOptionList(x => x, x => x, null);
    }

    public static string GetSelectedTypeName(this SkillFormViewStates states)
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = states.TypeOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(states.SkillType, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }
    #endregion
}