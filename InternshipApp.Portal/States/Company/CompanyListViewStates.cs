using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class CompanyListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<CompanyListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public CompanyListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public List<CompanyListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
