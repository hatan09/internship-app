using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class SkillScoreListViewStates : BaseViewModel
{
    #region [ Fields ]
    private List<Option> _options;
    #endregion

    #region [ CTor ]
    public SkillScoreListViewStates()
    {
        _options = new();
    }
    #endregion

    #region [ Properties ]

    public List<Option> Options
    {
        get { return this._options; }
        set { this.SetProperty(ref this._options, value); }
    }
    #endregion
}
