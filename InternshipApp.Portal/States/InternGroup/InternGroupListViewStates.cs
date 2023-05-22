using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class InternGroupListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<InternGroupListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public InternGroupListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public List<InternGroupListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
