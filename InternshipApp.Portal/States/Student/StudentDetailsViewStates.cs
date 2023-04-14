namespace InternshipApp.Portal.Views;

public class StudentDetailsViewStates : BaseStudentViewStates
{
	#region [ Fields ]

	private int _matching = 0;

	#endregion

	public StudentDetailsViewStates()
	{
		
	}

	#region [ Properties ]
	public int Matching
	{
		get { return this._matching; }
		set { this.SetProperty(ref this._matching, value); }
	}
	#endregion
}
