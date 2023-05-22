using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class InternGroupExtension
{
    #region [ Public Methods - ListRow ]
    public static List<InternGroupListRowViewStates> ToListRowList(this List<InternGroup> list)
    {
        var result = new List<InternGroupListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static InternGroupListRowViewStates ToListRow(this InternGroup entity)
    {

        var viewStates = ToViewStates<InternGroupListRowViewStates>(entity);

        return viewStates;
    }

    public static InternGroup ToEntity(this InternGroupListRowViewStates viewStates)
    {
        return ToEntity<InternGroup>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static InternGroupFormViewStates ToFormViewStates(this InternGroup entity)
    {

        var viewmodel = ToViewStates<InternGroupFormViewStates>(entity);

        return viewmodel;
    }

    public static InternGroup ToEntity(this InternGroupFormViewStates viewStates)
    {
        return ToEntity<InternGroup>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static InternGroupDetailsViewStates ToDetailsViewStates(this InternGroup entity)
    {
        var viewmodel = ToViewStates<InternGroupDetailsViewStates>(entity);

        return viewmodel;
    }

    public static InternGroup ToEntity(this InternGroupDetailsViewStates viewStates)
    {
        return ToEntity<InternGroup>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this InternGroup entity)

        where TBaseViewModel : BaseInternGroupViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

            InstructorId = entity.InstructorId,
            Title = entity.Title
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseInternGroupViewStates viewModel)
        where TEntity : InternGroup, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

            InstructorId = viewModel.InstructorId,
            Title = viewModel.Title,
        };
    }
    #endregion

    #region [ Methods - Options ]
    // Type
    public static List<Option<string>> InstructorOptions(this InternGroupFormViewStates states)
    {
        return states.Instructors.ToOptionList(x => x.Id, x => x.FullName, null);
    }

    public static string GetSelectedInstructorId(this InternGroupFormViewStates states)
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = states.InstructorOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(states.InstructorName, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }
    #endregion
}