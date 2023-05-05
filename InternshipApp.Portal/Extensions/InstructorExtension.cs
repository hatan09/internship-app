using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class InstructorExtension
{
    #region [ Public Methods - ListRow ]
    public static List<InstructorListRowViewStates> ToListRowList(this List<Instructor> list)
    {
        var result = new List<InstructorListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static InstructorListRowViewStates ToListRow(this Instructor entity)
    {

        var viewStates = ToViewStates<InstructorListRowViewStates>(entity);

        return viewStates;
    }

    public static Instructor ToEntity(this InstructorListRowViewStates viewStates)
    {
        return ToEntity<Instructor>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static InstructorFormViewStates ToFormViewStates(this Instructor entity)
    {

        var viewmodel = ToViewStates<InstructorFormViewStates>(entity);

        return viewmodel;
    }

    public static Instructor ToEntity(this InstructorFormViewStates viewStates)
    {
        return ToEntity<Instructor>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    //public static InstructorDetailsViewStates ToDetailsViewStates(this Instructor entity)
    //{

    //    var viewmodel = ToViewStates<InstructorDetailsViewStates>(entity);

    //    return viewmodel;
    //}

    //public static Instructor ToEntity(this InstructorDetailsViewStates viewStates)
    //{
    //    return ToEntity<Instructor>(viewStates);
    //}
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Instructor entity)

        where TBaseViewModel : BaseInstructorViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

            Name = entity.FullName,
            Email = entity.Email,

            Username = entity.UserName
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseInstructorViewStates viewModel)
        where TEntity : Instructor, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

            FullName = viewModel.Name,
            Email = viewModel.Email,

            UserName = viewModel.Username
        };
    }
    #endregion
}