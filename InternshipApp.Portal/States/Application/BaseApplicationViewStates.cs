using InternshipApp.Core.Entities;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseApplicationViewStates : BaseViewModel
{
    #region [ Fields ]
    private string _id;
    private string _studentId;
    private string _studentName;
    private int _year;
    private double _gpa;
    private double _credits;
    private int _jobId;
    private string _jobName;
    private double _matching;
    #endregion

    #region [ CTor ]
    public BaseApplicationViewStates()
    {
        _id = Guid.NewGuid().ToString();
        _skillIds = new();
    }
    #endregion

    #region [ Properties ]
    public string Id
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

    public int Year
    {
        get { return this._year; }
        set { this.SetProperty(ref this._year, value); }
    }

    public double Gpa
    {
        get { return this._gpa; }
        set { this.SetProperty(ref this._gpa, value); }
    }

    public double Credits
    {
        get { return this._credits; }
        set { this.SetProperty(ref this._credits, value); }
    }

    public int JobId
    {
        get { return this._jobId; }
        set { this.SetProperty(ref this._jobId, value); }
    }

    public string JobName
    {
        get { return this._jobName; }
        set { this.SetProperty(ref this._jobName, value); }
    }

    public double Matching
    {
        get { return this._matching; }
        set { this.SetProperty(ref this._matching, value); }
    }

    private ApplyStatus _status;
    public ApplyStatus Status
    {
        get { return this._status; }
        set { this.SetProperty(ref this._status, value); }
    }

    private List<int> _skillIds;
    public List<int> SkillIds
    {
        get { return this._skillIds; }
        set { this.SetProperty(ref this._skillIds, value); }
    }

    private string _bio;
    public string Bio
    {
        get { return this._bio; }
        set { this.SetProperty(ref this._bio, value); }
    }
    #endregion
}
