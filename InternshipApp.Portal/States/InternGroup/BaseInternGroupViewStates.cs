using InternshipApp.Core.Entities;
using RCode.ViewModels;

namespace InternshipApp.Portal.Views;

public class BaseInternGroupViewStates : BaseViewModel
{
    #region [ Fields ]
    private int _id;
    private string _title;
    private string _instructorId;
    private string _instructorName;
    private List<Instructor> _instructors;
    private string _selectedInstructor;
    private int _amount;
    #endregion

    #region [ CTor ]
    public BaseInternGroupViewStates()
    {
        _instructors = new();
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

    public string InstructorId
    {
        get { return this._instructorId; }
        set { this.SetProperty(ref this._instructorId, value); }
    }

    public string InstructorName
    {
        get { return this._instructorName; }
        set { this.SetProperty(ref this._instructorName, value); }
    }

    public List<Instructor> Instructors
    {
        get { return this._instructors; }
        set { this.SetProperty(ref this._instructors, value); }
    }

    public string SelectedInstructor
    {
        get { return this._selectedInstructor; }
        set { this.SetProperty(ref this._selectedInstructor, value); }
    }

    public int Amount
    {
        get { return this._amount; }
        set { this.SetProperty(ref this._amount, value); }
    }
    #endregion
}
