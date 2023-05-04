using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class StudentExtension
{
    #region [ Public Methods - ListRow ]
    public static List<StudentListRowViewStates> ToListRowList(this List<Student> list)
    {
        var result = new List<StudentListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static StudentListRowViewStates ToListRow(this Student entity)
    {

        var viewStates = ToViewStates<StudentListRowViewStates>(entity);

        return viewStates;
    }

    public static Student ToEntity(this StudentListRowViewStates viewStates)
    {
        return ToEntity<Student>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static StudentFormViewStates ToFormViewStates(this Student entity)
    {

        var viewmodel = ToViewStates<StudentFormViewStates>(entity);

        return viewmodel;
    }

    public static Student ToEntity(this StudentFormViewStates viewStates)
    {
        return ToEntity<Student>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static StudentDetailsViewStates ToDetailsViewStates(this Student entity)
    {

        var viewmodel = ToViewStates<StudentDetailsViewStates>(entity);

        return viewmodel;
    }

    public static Student ToEntity(this StudentDetailsViewStates viewStates)
    {
        return ToEntity<Student>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Student entity)

        where TBaseViewModel : BaseStudentViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

            Name = entity.FullName,
            Bio = entity.Bio,
            Gpa = entity.GPA,
            Year = entity.Year,
            Credits = entity.Credit,
            Status = entity.Stat.ToString(),
            StudentId = entity.StudentId,
            ImgUrl = entity.ImgUrl,
            GitUrl = entity.GitProfileUrl,
            CVUrl = entity.CVUrl,

            Username = entity.UserName
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseStudentViewStates viewModel)
        where TEntity : Student, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

            FullName = viewModel.Name,
            Bio = viewModel.Bio,
            Year = viewModel.Year,
            GPA = viewModel.Gpa,
            Credit = viewModel.Credits,
            Stat = (Stat)Enum.Parse(typeof(Stat), viewModel.Status, true),
            StudentId = viewModel.StudentId,
            ImgUrl = viewModel.ImgUrl,
            GitProfileUrl = viewModel.GitUrl,
            CVUrl = viewModel.CVUrl,

            UserName = viewModel.Username
        };
    }
    #endregion
}