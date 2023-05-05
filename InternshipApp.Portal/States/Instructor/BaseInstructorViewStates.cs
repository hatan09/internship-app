using RCode.ViewModels;

namespace InternshipApp.Portal.Views;


public class BaseInstructorViewStates : BaseViewModel
{
    #region [ Fields ]
    private string _id;
    private string _name;
    private string _email;
    private string _username;
    private string _password;
    private string _oldPassword;
    #endregion

    #region [ CTor ]
    public BaseInstructorViewStates()
    {
    }
    #endregion

    #region [ Properties ]
    public string Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public string Name
    {
        get { return this._name; }
        set { this.SetProperty(ref this._name, value); }
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
    #endregion
}
