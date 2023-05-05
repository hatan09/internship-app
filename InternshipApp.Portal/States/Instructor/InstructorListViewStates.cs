using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class InstructorListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<InstructorListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public InstructorListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public List<InstructorListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
