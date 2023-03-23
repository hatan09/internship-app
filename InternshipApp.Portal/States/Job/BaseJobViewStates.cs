using InternshipApp.Core.Entities;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseJobViewStates : BaseViewModel
{
    #region [ Fields ]
    private string _id;
    public int _credit;
    public double _gpa;
    public int _slots;
    public string _title;
    public string _companyName;
    public double _matching;
    public List<StudentSkill> _studentSkills;

    public Company Company { get; set; }
    #endregion

    #region [ CTor ]
    public BaseJobViewStates()
    {
        _studentSkills = new();
    }
    #endregion

    #region [ Properties ]
    public string Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public int Credit
    {
        get { return this._credit; }
        set { this.SetProperty(ref this._credit, value); }
    }

    public double Gpa
    {
        get { return this._gpa; }
        set { this.SetProperty(ref this._gpa, value); }
    }

    public int Slots
    {
        get { return this._slots; }
        set { this.SetProperty(ref this._slots, value); }
    }

    public string Title
    {
        get { return this._title; }
        set { this.SetProperty(ref this._title, value); }
    }

    public string CompanyName
    {
        get { return this._companyName; }
        set { this.SetProperty(ref this._companyName, value); }
    }

    public double Matching
    {
        get { return this._matching; }
        set { this.SetProperty(ref this._matching, value); }
    }
    #endregion
}
