using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseEvaluationViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private string _studentId;
    private string _studentName;
    private int _jobId;
    private string _jobName;
    #endregion

    #region [ CTor ]
    public BaseEvaluationViewStates()
    {
        
    }
    #endregion

    #region [ Properties ]
    public int Id
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

    private string _comment;
    public string Comment
    {
        get { return this._comment; }
        set { this.SetProperty(ref this._comment, value); }
    }

    private string _projectName;
    public string ProjectName
    {
        get { return this._projectName; }
        set { this.SetProperty(ref this._projectName, value); }
    }

    private int _score;
    public int Score
    {
        get { return this._score; }
        set { this.SetProperty(ref this._score, value); }
    }

    private string _performance;
    public string Performance
    {
        get { return this._performance; }
        set { this.SetProperty(ref this._performance, value); }
    }

    private DateTime _createdDate;
    public DateTime CreatedDate
    {
        get { return this._createdDate; }
        set { this.SetProperty(ref this._createdDate, value); }
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
    #endregion
}
