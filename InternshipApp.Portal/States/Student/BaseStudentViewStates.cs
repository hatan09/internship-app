using RCode.ViewModels;

namespace InternshipApp.Portal.Views;


public class BaseStudentViewStates : BaseViewModel
{
    #region [ Fields ]
    private string _id;
    private string _studentId;
    private string _name;
    private int _year;
    private double _gpa;
    private int _credits;
    private string _bio;
    private string _status;
    private string _username;
    private string _password;
    private string _imgUrl;
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

    public string ImgUrl
    {
        get { return this._imgUrl; }
        set { this.SetProperty(ref this._imgUrl, value); }
    }
    #endregion
}
