using RCode.ViewModels;

namespace InternshipApp.Portal.Views;


public class BaseRecruiterViewStates : BaseViewModel
{
    #region [ Fields ]
    private string _id;
    private int _companyId;
    private string _name;
    private string _email;
    private string _username;
    private string _password;
    private string _oldPassword;
    #endregion

    #region [ CTor ]
    public BaseRecruiterViewStates()
    {
    }
    #endregion

    #region [ Properties ]
    public string Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public int CompanyId
    {
        get { return this._companyId; }
        set { this.SetProperty(ref this._companyId, value); }
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
