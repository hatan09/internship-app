using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class RecruiterListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<RecruiterListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public RecruiterListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public List<RecruiterListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
