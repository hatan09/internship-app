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
    private string _companyName;
    private ApplyStatus _status;
    private List<int> _skillIds;
    private string _bio;
    private string _gitHubUrl;
    private string _cvUrl;
    private string _imgUrl;
    private int _studentFormId;
    private int _courseFormId;
    private bool _isFormsCompleted;
    private string _studentEmail;
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

    public ApplyStatus Status
    {
        get { return this._status; }
        set { this.SetProperty(ref this._status, value); }
    }

    public List<int> SkillIds
    {
        get { return this._skillIds; }
        set { this.SetProperty(ref this._skillIds, value); }
    }

    public string Bio
    {
        get { return this._bio; }
        set { this.SetProperty(ref this._bio, value); }
    }

    public string CompanyName
    {
        get { return this._companyName; }
        set { this.SetProperty(ref this._companyName, value); }
    }

    public string GitHubUrl
    {
        get { return this._gitHubUrl; }
        set { this.SetProperty(ref this._gitHubUrl, value); }
    }

    public string CvUrl
    {
        get { return this._cvUrl; }
        set { this.SetProperty(ref this._cvUrl, value); }
    }

    public string ImgUrl
    {
        get { return this._imgUrl; }
        set { this.SetProperty(ref this._imgUrl, value); }
    }

    public int StudentFormId
    {
        get { return this._studentFormId; }
        set { this.SetProperty(ref this._studentFormId, value); }
    }

    public int CourseFormId
    {
        get { return this._courseFormId; }
        set { this.SetProperty(ref this._courseFormId, value); }
    }

    public bool IsFormsCompleted
    {
        get { return this._isFormsCompleted; }
        set { this.SetProperty(ref this._isFormsCompleted, value); }
    }

    public string StudentEmail
    {
        get { return this._studentEmail; }
        set { this.SetProperty(ref this._studentEmail, value); }
    }
    #endregion
}
