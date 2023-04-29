using System.Collections.ObjectModel;
using InternshipApp.Core.Entities;
using RCode.UI.ViewModels;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseStudentSkillViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private string _studentId;
    private string _studentName;
    private int _skillId;
    private string _skillName;
    private string _level;
    private string _description;
    private ObservableCollection<Skill> _skills;
    private string _selectedSkill;
    private ObservableCollection<string> _levels;
    private string _selectedLevel;

    #endregion

    #region [ CTor ]
    public BaseStudentSkillViewStates()
    {
        var ran = new Random();
        _id = ran.Next(1, 99999);
        _skills = new ObservableCollection<Skill>();
        _levels = Enum.GetValues(typeof(SkillLevel)).Cast<SkillLevel>().Select(x => x.ToString()).ToList().ToObservableCollection();
    }
    #endregion

    #region [ Properties ]
    public int Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public string StudentId
    {
        get { return this._studentId; }
        set { this.SetProperty(ref this._studentId, value); }
    }

    public string StudentName
    {
        get { return this._studentName; }
        set { this.SetProperty(ref this._studentName, value); }
    }

    public int SkillId
    {
        get { return this._skillId; }
        set { this.SetProperty(ref this._skillId, value); }
    }

    public string Level
    {
        get { return this._level; }
        set { this.SetProperty(ref this._level, value); }
    }

    public string SkillName
    {
        get { return this._skillName; }
        set { this.SetProperty(ref this._skillName, value); }
    }

    public string Description
    {
        get { return this._description; }
        set { this.SetProperty(ref this._description, value); }
    }

    public ObservableCollection<Skill> Skills
    {
        get { return this._skills; }
        set { this.SetProperty(ref this._skills, value); }
    }

    public string SelectedSkill
    {
        get { return this._selectedSkill; }
        set { this.SetProperty(ref this._selectedSkill, value); }
    }

    public ObservableCollection<string> Levels
    {
        get { return this._levels; }
        set { this.SetProperty(ref this._levels, value); }
    }

    public string SelectedLevel
    {
        get { return this._selectedLevel; }
        set { this.SetProperty(ref this._selectedLevel, value); }
    }
    #endregion
}
