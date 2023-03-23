using InternshipApp.Core.Entities;

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
    //public static StudentFormViewStates ToFormViewStates(this Student entity) {

    //    var viewmodel = ToViewStates<StudentFormViewStates>(entity);

    //    return viewmodel;
    //}

    //public static Student ToEntity(this StudentFormViewStates viewStates) {
    //    return ToEntity<Student>(viewStates);
    //}
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    //public static StudentDetailsViewStates ToDetailsViewStates(this Student entity)
    //{

    //    var viewmodel = ToViewStates<StudentDetailsViewStates>(entity);

    //    return viewmodel;
    //}

    //public static Student ToEntity(this StudentDetailsViewStates viewStates)
    //{
    //    return ToEntity<Student>(viewStates);
    //}
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Student entity)

        where TBaseViewModel : BaseStudentViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

            Name = entity.FullName,
            Gpa = entity.GPA,
            Status = entity.Stat.ToString(),
            StudentId = entity.StudentId
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseStudentViewStates viewModel)
        where TEntity : Student, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

            FullName = viewModel.Name,
            GPA = viewModel.Gpa,
            Stat = (Stat)Enum.Parse(typeof(Stat), viewModel.Status, true),
            StudentId = viewModel.StudentId
        };
    }
    #endregion
}