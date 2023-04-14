using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public static class CompanyExtension
{
    #region [ Public Methods - ListRow ]
    public static List<CompanyListRowViewStates> ToListRowList(this List<Company> list)
    {
        var result = new List<CompanyListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static CompanyListRowViewStates ToListRow(this Company entity)
    {

        var viewStates = ToViewStates<CompanyListRowViewStates>(entity);

        return viewStates;
    }

    public static Company ToEntity(this CompanyListRowViewStates viewStates)
    {
        return ToEntity<Company>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    //public static CompanyFormViewStates ToFormViewStates(this Company entity) {

    //    var viewmodel = ToViewStates<CompanyFormViewStates>(entity);

    //    return viewmodel;
    //}

    //public static Company ToEntity(this CompanyFormViewStates viewStates) {
    //    return ToEntity<Company>(viewStates);
    //}
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static CompanyDetailsViewStates ToDetailsViewStates(this Company entity)
    {
        var viewmodel = ToViewStates<CompanyDetailsViewStates>(entity);

        return viewmodel;
    }

    public static Company ToEntity(this CompanyDetailsViewStates viewStates)
    {
        return ToEntity<Company>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Company entity)

        where TBaseViewModel : BaseCompanyViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

        };
    }

    private static TEntity ToEntity<TEntity>(this BaseCompanyViewStates viewModel)
        where TEntity : Company, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

        };
    }
    #endregion
}