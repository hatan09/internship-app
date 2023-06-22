using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public static class CourseFormExtension
{
    #region [ Public Methods - ListRow ]
    public static List<CourseFormListRowViewStates> ToListRowList(this List<CourseForm> list)
    {
        var result = new List<CourseFormListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static CourseFormListRowViewStates ToListRow(this CourseForm entity)
    {

        var viewStates = ToViewStates<CourseFormListRowViewStates>(entity);

        return viewStates;
    }

    public static CourseForm ToEntity(this CourseFormListRowViewStates viewStates)
    {
        return ToEntity<CourseForm>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static CourseFormFormViewStates ToFormViewStates(this CourseForm entity)
    {

        var viewmodel = ToViewStates<CourseFormFormViewStates>(entity);

        return viewmodel;
    }

    public static CourseForm ToEntity(this CourseFormFormViewStates viewStates)
    {
        return ToEntity<CourseForm>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static CourseFormDetailsViewStates ToDetailsViewStates(this CourseForm entity)
    {

        var viewmodel = ToViewStates<CourseFormDetailsViewStates>(entity);

        return viewmodel;
    }

    public static CourseForm ToEntity(this CourseFormDetailsViewStates viewStates)
    {
        return ToEntity<CourseForm>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this CourseForm entity)

        where TBaseViewModel : BaseCourseFormViewStates, new()
    {
        return new TBaseViewModel()
        {

        };
    }

    private static TEntity ToEntity<TEntity>(this BaseCourseFormViewStates viewModel)
        where TEntity : CourseForm, new()
    {
        return new TEntity()
        {

        };
    }
    #endregion
}