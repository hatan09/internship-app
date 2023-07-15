using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public static class CourseFormExtension
{
    #region [ Public Methods - ListRow ]
    public static List<CourseFormListRowViewStates> ToListRowList(this List<LabourMarketForm> list)
    {
        var result = new List<CourseFormListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static CourseFormListRowViewStates ToListRow(this LabourMarketForm entity)
    {

        var viewStates = ToViewStates<CourseFormListRowViewStates>(entity);

        return viewStates;
    }

    public static LabourMarketForm ToEntity(this CourseFormListRowViewStates viewStates)
    {
        return ToEntity<LabourMarketForm>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static CourseFormFormViewStates ToFormViewStates(this LabourMarketForm entity)
    {

        var viewmodel = ToViewStates<CourseFormFormViewStates>(entity);

        return viewmodel;
    }

    public static LabourMarketForm ToEntity(this CourseFormFormViewStates viewStates)
    {
        return ToEntity<LabourMarketForm>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static CourseFormDetailsViewStates ToDetailsViewStates(this LabourMarketForm entity)
    {

        var viewmodel = ToViewStates<CourseFormDetailsViewStates>(entity);

        return viewmodel;
    }

    public static LabourMarketForm ToEntity(this CourseFormDetailsViewStates viewStates)
    {
        return ToEntity<LabourMarketForm>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this LabourMarketForm entity)

        where TBaseViewModel : BaseCourseFormViewStates, new()
    {
        return new TBaseViewModel()
        {

        };
    }

    private static TEntity ToEntity<TEntity>(this BaseCourseFormViewStates viewModel)
        where TEntity : LabourMarketForm, new()
    {
        return new TEntity()
        {

        };
    }
    #endregion
}