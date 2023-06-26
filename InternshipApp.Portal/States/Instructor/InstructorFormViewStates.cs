using RCode.ViewModels;
using System.Collections.ObjectModel;

namespace InternshipApp.Portal.Views;

public class InstructorFormViewStates : BaseInstructorViewStates
{
    #region [ Fields ]
    private bool _isManager;
    #endregion

    #region [ CTor ]
    public InstructorFormViewStates()
    {
    }
    #endregion

    #region [ Properties ]
    public bool IsManager
    {
        get { return this._isManager; }
        set { this.SetProperty(ref this._isManager, value); }
    }
    #endregion
}
