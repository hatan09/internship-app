using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class StudentListViewStates : BaseViewModel
{
    #region [ Fields ]
    private ObservableCollection<StudentListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public StudentListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public ObservableCollection<StudentListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
