using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

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
            Description= entity.Description,
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
}
