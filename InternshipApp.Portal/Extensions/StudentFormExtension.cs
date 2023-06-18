using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public static class StudentFormExtension
{
    #region [ Public Methods - ListRow ]
    public static List<StudentFormListRowViewStates> ToListRowList(this List<StudentForm> list)
    {
        var result = new List<StudentFormListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static StudentFormListRowViewStates ToListRow(this StudentForm entity)
    {

        var viewStates = ToViewStates<StudentFormListRowViewStates>(entity);

        return viewStates;
    }

    public static StudentForm ToEntity(this StudentFormListRowViewStates viewStates)
    {
        return ToEntity<StudentForm>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static StudentFormFormViewStates ToFormViewStates(this StudentForm entity)
    {

        var viewmodel = ToViewStates<StudentFormFormViewStates>(entity);

        return viewmodel;
    }

    public static StudentForm ToEntity(this StudentFormFormViewStates viewStates)
    {
        return ToEntity<StudentForm>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static StudentFormDetailsViewStates ToDetailsViewStates(this StudentForm entity)
    {

        var viewmodel = ToViewStates<StudentFormDetailsViewStates>(entity);

        return viewmodel;
    }

    public static StudentForm ToEntity(this StudentFormDetailsViewStates viewStates)
    {
        return ToEntity<StudentForm>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this StudentForm entity)

        where TBaseViewModel : BaseStudentFormViewStates, new()
    {
        return new TBaseViewModel()
        {

        };
    }

    private static TEntity ToEntity<TEntity>(this BaseStudentFormViewStates viewModel)
        where TEntity : StudentForm, new()
    {
        return new TEntity()
        {

        };
    }
    #endregion
}