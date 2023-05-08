using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI;

namespace InternshipApp.Portal.Views;

public static class SkillScoreExtension
{
    #region [ Public Methods - ListRow ]
    public static List<SkillScoreListRowViewStates> ToListRowList(this List<SkillScore> list)
    {
        var result = new List<SkillScoreListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static SkillScoreListRowViewStates ToListRow(this SkillScore entity)
    {

        var viewStates = ToViewStates<SkillScoreListRowViewStates>(entity);

        return viewStates;
    }

    public static SkillScore ToEntity(this SkillScoreListRowViewStates viewStates)
    {
        return ToEntity<SkillScore>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static SkillScoreFormViewStates ToFormViewStates(this SkillScore entity)
    {

        var viewmodel = ToViewStates<SkillScoreFormViewStates>(entity);

        return viewmodel;
    }

    public static SkillScore ToEntity(this SkillScoreFormViewStates viewStates)
    {
        return ToEntity<SkillScore>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    //public static SkillScoreDetailsViewStates ToDetailsViewStates(this SkillScore entity)
    //{
    //    var viewmodel = ToViewStates<SkillScoreDetailsViewStates>(entity);

    //    return viewmodel;
    //}

    //public static SkillScore ToEntity(this SkillScoreDetailsViewStates viewStates)
    //{
    //    return ToEntity<SkillScore>(viewStates);
    //}
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this SkillScore entity)

        where TBaseViewModel : BaseSkillScoreViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

        };
    }

    private static TEntity ToEntity<TEntity>(this BaseSkillScoreViewStates viewModel)
        where TEntity : SkillScore, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

        };
    }
    #endregion

    #region [ Methods - Options ]
    // Type
    public static List<Option<string>> TypeOptions(this SkillScoreFormViewStates states)
    {
        return states.Types.ToOptionList(x => x, x => x, null);
    }

    public static string GetSelectedTypeName(this SkillScoreFormViewStates states)
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = states.TypeOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(states.SkillScoreType, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }
    #endregion
}