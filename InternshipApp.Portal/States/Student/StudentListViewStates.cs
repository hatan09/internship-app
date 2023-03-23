using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class StudentListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<StudentListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public StudentListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public List<StudentListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
