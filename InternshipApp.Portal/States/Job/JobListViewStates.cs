using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class JobListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<JobListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public JobListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public List<JobListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
