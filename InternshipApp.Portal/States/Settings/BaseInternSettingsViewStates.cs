using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseInternSettingsViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private string _title;
    private DateTime _start;
    private DateTime _closeReg;
    private DateTime _jobDeadline;
    private DateTime _summary;
    private DateTime _end;
    #endregion

    #region [ CTor ]
    public BaseInternSettingsViewStates()
    {

    }
    #endregion

    #region [ Properties ]
    public int Id
    {
        get { return this._id; }
        set { this.SetProperty(ref this._id, value); }
    }

    public string Title
    {
        get { return this._title; }
        set { this.SetProperty(ref this._title, value); }
    }

    public DateTime Start
    {
        get { return this._start; }
        set { this.SetProperty(ref this._start, value); }
    }

    public DateTime CloseReg
    {
        get { return this._closeReg; }
        set { this.SetProperty(ref this._closeReg, value); }
    }

    public DateTime JobDeadline
    {
        get { return this._jobDeadline; }
        set { this.SetProperty(ref this._jobDeadline, value); }
    }

    public DateTime Summary
    {
        get { return this._summary; }
        set { this.SetProperty(ref this._summary, value); }
    }

    public DateTime End
    {
        get { return this._end; }
        set { this.SetProperty(ref this._end, value); }
    }
    #endregion
}
