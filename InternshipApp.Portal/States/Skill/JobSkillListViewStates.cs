using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class JobSkillListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<Option> _options;
    private List<JobSkillListRowViewStates> _items;
    private List<JobSkillListRowViewStates> _originalItems;
    #endregion

    #region [ CTor ]
    public JobSkillListViewStates()
    {
        _items = new();
        _options = new();
    }
    #endregion

    #region [ Properties ]
    public List<JobSkillListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }

    public List<JobSkillListRowViewStates> OriginalItems
    {
        get { return this._originalItems; }
        set { this.SetProperty(ref this._originalItems, value); }
    }

    public List<Option> Options
    {
        get { return this._options; }
        set { this.SetProperty(ref this._options, value); }
    }
    #endregion
}
