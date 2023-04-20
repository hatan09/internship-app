using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class ApplicationExtension
{
    #region [ Public Methods - ListRow ]
    public static List<ApplicationListRowViewStates> ToListRowList(this List<StudentJob> list)
    {
        var result = new List<ApplicationListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static ApplicationListRowViewStates ToListRow(this StudentJob entity)
    {

        var viewStates = ToViewStates<ApplicationListRowViewStates>(entity);

        return viewStates;
    }

    public static StudentJob ToEntity(this ApplicationListRowViewStates viewStates)
    {
        return ToEntity<StudentJob>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    //public static ApplicationFormViewStates ToFormViewStates(this Application entity) {

    //    var viewmodel = ToViewStates<ApplicationFormViewStates>(entity);

    //    return viewmodel;
    //}

    //public static Application ToEntity(this ApplicationFormViewStates viewStates) {
    //    return ToEntity<Application>(viewStates);
    //}
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    //public static ApplicationDetailsViewStates ToDetailsViewStates(this Application entity)
    //{

    //    var viewmodel = ToViewStates<ApplicationDetailsViewStates>(entity);

    //    return viewmodel;
    //}

    //public static Application ToEntity(this ApplicationDetailsViewStates viewStates)
    //{
    //    return ToEntity<Application>(viewStates);
    //}
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this StudentJob entity)

        where TBaseViewModel : BaseApplicationViewStates, new()
    {
        return new TBaseViewModel()
        {
            StudentId = entity.StudentId,


        };
    }

    private static TEntity ToEntity<TEntity>(this BaseApplicationViewStates viewModel)
        where TEntity : StudentJob, new()
    {
        return new TEntity()
        {
            StudentId = viewModel.StudentId,


        };
    }
    #endregion
}