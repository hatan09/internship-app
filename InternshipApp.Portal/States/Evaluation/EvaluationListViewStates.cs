using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class EvaluationListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<EvaluationListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public EvaluationListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]
    public List<EvaluationListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
