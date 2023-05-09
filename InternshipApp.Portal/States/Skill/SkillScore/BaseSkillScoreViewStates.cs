using System.Collections.ObjectModel;
using InternshipApp.Core.Entities;
using RCode.UI.ViewModels;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseSkillScoreViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private int _masterSkillId;
    private string _masterSkillName;
    private int _skillId;
    private string _skillName;
    private ObservableCollection<Skill> _skills;
    private ObservableCollection<string> _types;
    private string _matchType;
    private string _category;

    #endregion

    #region [ CTor ]
    public BaseSkillScoreViewStates()
    {
        _types = Enum.GetValues(typeof(MatchingType)).Cast<MatchingType>().Select(x => x.ToString()).ToList().ToObservableCollection();
        _skills = new();
    }
    #endregion

    #region [ Properties ]
    public int Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public int MasterSkillId
    {
        get { return this._masterSkillId; }
        set { this.SetProperty(ref this._masterSkillId, value); }
    }

    public string MasterSkillName
    {
        get { return this._masterSkillName; }
        set { this.SetProperty(ref this._masterSkillName, value); }
    }

    public int SkillId
    {
        get { return this._skillId; }
        set { this.SetProperty(ref this._skillId, value); }
    }

    public string Name
    {
        get { return this._skillName; }
        set { this.SetProperty(ref this._skillName, value); }
    }

    public ObservableCollection<Skill> Skills
    {
        get { return this._skills; }
        set { this.SetProperty(ref this._skills, value); }
    }

    private int _selectedSkillId;
    public int SelectedSkillId
    {
        get { return this._selectedSkillId; }
        set { this.SetProperty(ref this._selectedSkillId, value); }
    }

    public ObservableCollection<string> Types
    {
        get { return this._types; }
        set { this.SetProperty(ref this._types, value); }
    }

    public string MatchingType
    {
        get { return this._matchType; }
        set { this.SetProperty(ref this._matchType, value); }
    }

    public string Category
    {
        get { return this._category; }
        set { this.SetProperty(ref this._category, value); }
    }
    #endregion
}
