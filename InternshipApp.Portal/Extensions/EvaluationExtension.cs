using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class EvaluationExtension
{
    #region [ Public Methods - ListRow ]
    public static List<EvaluationListRowViewStates> ToListRowList(this List<Evaluation> list)
    {
        var result = new List<EvaluationListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static EvaluationListRowViewStates ToListRow(this Evaluation entity)
    {

        var viewStates = ToViewStates<EvaluationListRowViewStates>(entity);

        return viewStates;
    }

    public static Evaluation ToEntity(this EvaluationListRowViewStates viewStates)
    {
        return ToEntity<Evaluation>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static EvaluationFormViewStates ToFormViewStates(this Evaluation entity)
    {

        var viewmodel = ToViewStates<EvaluationFormViewStates>(entity);

        return viewmodel;
    }

    public static Evaluation ToEntity(this EvaluationFormViewStates viewStates)
    {
        return ToEntity<Evaluation>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Evaluation entity)

        where TBaseViewModel : BaseEvaluationViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = (int) entity.Id,
            StudentId = entity.StudentId,
            JobId = (int) (entity.JobId?? 0),
            ProjectName = entity.ProjectName,
            Title = entity.Title,
            Comment = entity.Comment,
            CreatedDate = entity.Date.Date,
            Performance = entity.Performance.ToString(),
            Score = entity.Score,
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseEvaluationViewStates viewModel)
        where TEntity : Evaluation, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,
            StudentId = viewModel.StudentId,
            JobId = viewModel.JobId,
            ProjectName = viewModel.ProjectName,
            Title = viewModel.Title,
            Comment = viewModel.Comment,
            Date = viewModel.CreatedDate.Date,
            Performance = Enum.Parse<PerformanceRank>(viewModel.Performance),
            Score = viewModel.Score,
        };
    }
    #endregion

    #region [ Public Methods - Options ]
    public static List<Option<string>> PerformanceOptions(this EvaluationFormViewStates states)
    {
        return states.PerformanceList.ToOptionList(x => x, x => x, null);
    }

    public static string GetSelectedPerformanceName(this EvaluationFormViewStates states)
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = states.PerformanceOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(states.Performance, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }
    #endregion
}