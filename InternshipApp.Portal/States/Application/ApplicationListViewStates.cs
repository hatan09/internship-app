using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class ApplicationListViewStates : BaseViewModel
{
    #region [ Fields ]
    private ObservableCollection<ApplicationListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public ApplicationListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public ObservableCollection<ApplicationListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
