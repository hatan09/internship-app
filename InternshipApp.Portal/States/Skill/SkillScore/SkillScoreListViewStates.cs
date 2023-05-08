using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class SkillScoreListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<SkillScoreListRowViewStates> _items;
    #endregion

    #region [ CTor ]
    public SkillScoreListViewStates()
    {
        _items = new();
    }
    #endregion

    #region [ Properties ]

    public List<SkillScoreListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }
    #endregion
}
