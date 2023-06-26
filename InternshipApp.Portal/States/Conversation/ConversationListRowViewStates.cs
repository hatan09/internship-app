using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class ConversationListRowViewStates : BaseViewModel
{
    #region [ Fields ]
    private string _id;
    private string _avatar;
    private string _name;
    private DateTime _lastMsgTime;
    private string _lastMsg;
    #endregion

    #region [ CTor ]
    public ConversationListRowViewStates()
    {

    }
    #endregion

    #region [ Properties ]
    public string Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public string Avatar
    {
        get { return this._avatar; }
        set { this.SetProperty(ref this._avatar, value); }
    }

    public string Name
    {
        get { return this._name; }
        set { this.SetProperty(ref this._name, value); }
    }

    public DateTime LastMsgTime
    {
        get { return this._lastMsgTime; }
        set { this.SetProperty(ref this._lastMsgTime, value); }
    }

    public string LastMsg
    {
        get { return this._lastMsg; }
        set { this.SetProperty(ref this._lastMsg, value); }
    }
    #endregion
}
