using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class RecruiterExtension
{
    #region [ Public Methods - ListRow ]
    public static List<RecruiterListRowViewStates> ToListRowList(this List<Recruiter> list)
    {
        var result = new List<RecruiterListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static RecruiterListRowViewStates ToListRow(this Recruiter entity)
    {

        var viewStates = ToViewStates<RecruiterListRowViewStates>(entity);

        return viewStates;
    }

    public static Recruiter ToEntity(this RecruiterListRowViewStates viewStates)
    {
        return ToEntity<Recruiter>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static RecruiterFormViewStates ToFormViewStates(this Recruiter entity)
    {

        var viewmodel = ToViewStates<RecruiterFormViewStates>(entity);

        return viewmodel;
    }

    public static Recruiter ToEntity(this RecruiterFormViewStates viewStates)
    {
        return ToEntity<Recruiter>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    //public static RecruiterDetailsViewStates ToDetailsViewStates(this Recruiter entity)
    //{

    //    var viewmodel = ToViewStates<RecruiterDetailsViewStates>(entity);

    //    return viewmodel;
    //}

    //public static Recruiter ToEntity(this RecruiterDetailsViewStates viewStates)
    //{
    //    return ToEntity<Recruiter>(viewStates);
    //}
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Recruiter entity)

        where TBaseViewModel : BaseRecruiterViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

            CompanyId = entity.CompanyId?? 0,
            Name = entity.FullName,
            Email = entity.Email,

            Username = entity.UserName
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseRecruiterViewStates viewModel)
        where TEntity : Recruiter, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

            CompanyId = viewModel.CompanyId,
            FullName = viewModel.Name,
            Email = viewModel.Email,

            UserName = viewModel.Username
        };
    }
    #endregion
}