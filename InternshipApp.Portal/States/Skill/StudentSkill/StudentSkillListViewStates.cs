using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class StudentSkillListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<JobFilterOption> _options;
    private List<StudentSkillListRowViewStates> _items;
    private List<StudentSkillListRowViewStates> _originalItems;
    #endregion

    #region [ CTor ]
    public StudentSkillListViewStates()
    {
        _items = new();
        _options = new();
    }
    #endregion

    #region [ Properties ]
    public List<StudentSkillListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }

    public List<StudentSkillListRowViewStates> OriginalItems
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
