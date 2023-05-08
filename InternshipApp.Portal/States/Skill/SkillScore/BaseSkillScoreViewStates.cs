using System.Collections.ObjectModel;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseSkillScoreViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private string _name;
    private ObservableCollection<string> _types;
    private string _skillType;

    #endregion

    #region [ CTor ]
    public BaseSkillScoreViewStates()
    {
        //_types = Enum.GetValues(typeof(SkillScoreType)).Cast<SkillScoreType>().Select(x => x.ToString()).ToList().ToObservableCollection();
    }
    #endregion

    #region [ Properties ]
    public int Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public string Name
    {
        get { return this._name; }
        set { this.SetProperty(ref this._name, value); }
    }

    public ObservableCollection<string> Types
    {
        get { return this._types; }
        set { this.SetProperty(ref this._types, value); }
    }

    public string SkillScoreType
    {
        get { return this._skillType; }
        set { this.SetProperty(ref this._skillType, value); }
    }
    #endregion
}
