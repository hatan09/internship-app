using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class JobListViewStates : BaseViewModel
{
    #region [ Fields ]
    private ObservableCollection<JobListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public JobListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public ObservableCollection<JobListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
