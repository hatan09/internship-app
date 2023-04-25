using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public static class JobExtension
{
    #region [ Public Methods - ListRow ]
    public static List<JobListRowViewStates> ToListRowList(this List<Job> list)
    {
        var result = new List<JobListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static JobListRowViewStates ToListRow(this Job entity)
    {

        var viewStates = ToViewStates<JobListRowViewStates>(entity);

        return viewStates;
    }

    public static Job ToEntity(this JobListRowViewStates viewStates)
    {
        return ToEntity<Job>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static JobFormViewStates ToFormViewStates(this Job entity)
    {

        var viewmodel = ToViewStates<JobFormViewStates>(entity);

        return viewmodel;
    }

    public static Job ToEntity(this JobFormViewStates viewStates)
    {
        return ToEntity<Job>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static JobDetailsViewStates ToDetailsViewStates(this Job entity)
    {

        var viewmodel = ToViewStates<JobDetailsViewStates>(entity);

        return viewmodel;
    }

    public static Job ToEntity(this JobDetailsViewStates viewStates)
    {
        return ToEntity<Job>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Job entity)

        where TBaseViewModel : BaseJobViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CompanyName = entity.Company?.Title,
            Credit = entity.MinCredit,
            Gpa = entity.MinGPA,
            Slots = entity.Slots,
            Year = entity.MinYear,
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseJobViewStates viewModel)
        where TEntity : Job, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Description = viewModel.Description,
            MinCredit = viewModel.Credit,
            MinGPA = viewModel.Gpa,
            Slots = viewModel.Slots,
            MinYear = viewModel.Year,
        };
    }
    #endregion
}