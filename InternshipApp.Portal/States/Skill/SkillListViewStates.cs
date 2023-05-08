using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class SkillListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<JobFilterOption> _options;
    private List<SkillListRowViewStates> _items;
    private List<SkillListRowViewStates> _originalItems;
    #endregion

    #region [ CTor ]
    public SkillListViewStates()
    {
        _items = new();
        _options = new();
    }
    #endregion

    #region [ Properties ]
    public List<SkillListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }

    public List<SkillListRowViewStates> OriginalItems
    {
        get { return this._originalItems; }
        set { this.SetProperty(ref this._originalItems, value); }
    }

    public List<JobFilterOption> Options
    {
        get { return this._options; }
        set { this.SetProperty(ref this._options, value); }
    }
    #endregion
}
