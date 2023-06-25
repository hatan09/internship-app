using InternshipApp.Core.Entities;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;


public class BaseStudentViewStates : BaseViewModel
{
    #region [ Fields ]
    private string _id;
    private int _internGroupId;
    private string _internGroupName;
    private string _studentId;
    private string _name;
    private int _year;
    private double _gpa;
    private int _credits;
    private string _bio;
    private string _status;
    private string _email;
    private string _username;
    private string _password;
    private string _oldPassword;
    private string _imgUrl;
    private string _gitUrl;
    private string _cVUrl;
    private string _jobName;
    private string _companyName;
    private string _newMessege;
    private List<StudentSkill> _studentSkills;
    #endregion

    #region [ CTor ]
    public BaseStudentViewStates()
    {
    }
    #endregion

    #region [ Properties ]
    public string Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public int InternGroupId
    {
        get { return this._internGroupId; }
        set { this.SetProperty(ref this._internGroupId, value); }
    }

    public string InternGroupName
    {
        get { return this._internGroupName; }
        set { this.SetProperty(ref this._internGroupName, value); }
    }

    public string StudentId
    {
        get { return this._studentId; }
        set { this.SetProperty(ref this._studentId, value); }
    }

    public string Name
    {
        get { return this._name; }
        set { this.SetProperty(ref this._name, value); }
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

    public int Credits
    {
        get { return this._credits; }
        set { this.SetProperty(ref this._credits, value); }
    }

    public string Bio
    {
        get { return this._bio; }
        set { this.SetProperty(ref this._bio, value); }
    }

    public string Status
    {
        get { return this._status; }
        set { this.SetProperty(ref this._status, value); }
    }

    public string Email
    {
        get { return this._email; }
        set { this.SetProperty(ref this._email, value); }
    }

    public string Username
    {
        get { return this._username; }
        set { this.SetProperty(ref this._username, value); }
    }

    public string Password
    {
        get { return this._password; }
        set { this.SetProperty(ref this._password, value); }
    }

    public string OldPassword
    {
        get { return this._oldPassword; }
        set { this.SetProperty(ref this._oldPassword, value); }
    }

    public string ImgUrl
    {
        get { return this._imgUrl; }
        set { this.SetProperty(ref this._imgUrl, value); }
    }

    public string GitUrl
    {
        get { return this._gitUrl; }
        set { this.SetProperty(ref this._gitUrl, value); }
    }

    public string CVUrl
    {
        get { return this._cVUrl; }
        set { this.SetProperty(ref this._cVUrl, value); }
    }

    public string JobName
    {
        get { return this._jobName; }
        set { this.SetProperty(ref this._jobName, value); }
    }

    public string CompanyName
    {
        get { return this._companyName; }
        set { this.SetProperty(ref this._companyName, value); }
    }

    public string NewMessege
    {
        get { return this._newMessege; }
        set { this.SetProperty(ref this._newMessege, value); }
    }

    public List<StudentSkill> StudentSkills
    {
        get { return this._studentSkills; }
        set { this.SetProperty(ref this._studentSkills, value); }
    }
    #endregion
}
