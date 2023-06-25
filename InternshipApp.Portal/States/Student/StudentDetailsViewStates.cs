using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.Views;

public class StudentDetailsViewStates : BaseStudentViewStates
{
	#region [ Fields ]
	private int _matching = 0;
	private List<Skill> _allSkills;
    #endregion

    public StudentDetailsViewStates()
	{
		
	}

	#region [ Properties ]
	public List<Skill> AllSkills
	{
		get { return this._allSkills; }
		set { this.SetProperty(ref this._allSkills, value); }
	}

	public int Matching
	{
		get { return this._matching; }
		set { this.SetProperty(ref this._matching, value); }
	}
	#endregion
}
