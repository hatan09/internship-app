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
    private string _special;
    private string _status;
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

    public string Special
    {
        get { return this._special; }
        set { this.SetProperty(ref this._special, value); }
    }

    public string Status
    {
        get { return this._status; }
        set { this.SetProperty(ref this._status, value); }
    }
    #endregion
}
