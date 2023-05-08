using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class JobListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<JobFilterOption> _options;
    private List<JobListRowViewStates> _items;
    private List<JobListRowViewStates> _originalItems;
    #endregion

    #region [ CTor ]
    public JobListViewStates()
    {
        _items = new();
        _options = new();
    }
    #endregion

    #region [ Properties ]
    public List<JobListRowViewStates> Items
    {
        get { return this._items; }
        set { this.SetProperty(ref this._items, value); }
    }

    public List<JobListRowViewStates> OriginalItems
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

public class JobFilterOption
{
    public string Id { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }

}
