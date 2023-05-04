using InternshipApp.Core.Entities;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace InternshipApp.Portal.Views;

public static class CompanyExtension
{
    #region [ Public Methods - ListRow ]
    public static List<CompanyListRowViewStates> ToListRowList(this List<Company> list)
    {
        var result = new List<CompanyListRowViewStates>();
        list.ForEach(x => result.Add(x.ToListRow()));
        return result;
    }

    public static CompanyListRowViewStates ToListRow(this Company entity)
    {

        var viewStates = ToViewStates<CompanyListRowViewStates>(entity);

        return viewStates;
    }

    public static Company ToEntity(this CompanyListRowViewStates viewStates)
    {
        return ToEntity<Company>(viewStates);
    }
    #endregion

    #region [ Public Methods - FormViewModel ]
    public static CompanyFormViewStates ToFormViewStates(this Company entity)
    {

        var viewmodel = ToViewStates<CompanyFormViewStates>(entity);

        return viewmodel;
    }

    public static Company ToEntity(this CompanyFormViewStates viewStates)
    {
        return ToEntity<Company>(viewStates);
    }
    #endregion

    #region [ Public Methods - DetailsViewModel ]
    public static CompanyDetailsViewStates ToDetailsViewStates(this Company entity)
    {
        var viewmodel = ToViewStates<CompanyDetailsViewStates>(entity);

        return viewmodel;
    }

    public static Company ToEntity(this CompanyDetailsViewStates viewStates)
    {
        return ToEntity<Company>(viewStates);
    }
    #endregion

    #region [ Private Methods - BaseViewModel ]
    private static TBaseViewModel ToViewStates<TBaseViewModel>(this Company entity)

        where TBaseViewModel : BaseCompanyViewStates, new()
    {
        return new TBaseViewModel()
        {
            Id = entity.Id,

            Title = entity.Title,
            Address = entity.Address,
            CompanyWebsite = entity.CompanyWebsite,
            ImgUrl = entity.ImgUrl,
            CompanyType = entity.Type.ToString(),
            Description = entity.Description,
        };
    }

    private static TEntity ToEntity<TEntity>(this BaseCompanyViewStates viewModel)
        where TEntity : Company, new()
    {
        return new TEntity()
        {
            Id = viewModel.Id,

            Title = viewModel.Title,
            Address = viewModel.Address,
            CompanyWebsite = viewModel.CompanyWebsite,
            Description = viewModel.Description,
            ImgUrl = viewModel.ImgUrl,
            Type = Enum.Parse<CompanyType>(viewModel.CompanyType)
        };
    }
    #endregion

    #region [ Methods - Options ]
    // Type
    public static List<Option<string>> TypeOptions(this CompanyFormViewStates states)
    {
        return states.Types.ToOptionList(x => x, x => x, null);
    }

    public static string GetSelectedTypeName(this CompanyFormViewStates states)
    {
        // Somehow the SelectedProjectOption and ProjectOptions.Value (DisplayedName) might be different due to spaces 
        // ex: SelectedProjectOption: 'Roo - Koppeling DM'
        //     ProjectOptions.Value : 'Roo  - Koppeling DM'
        // solution: ProjectOptions.Value: replace multiple spaces by single spaces before doing comparision

        var result = states.TypeOptions()
                           .FirstOrDefault(x => x.Value.Trim().Equals(states.CompanyType, StringComparison.InvariantCultureIgnoreCase))?.Key;
        return result;
    }
    #endregion
}