using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class CompanyInfoPage
{
	#region [ Properties ]
	[Parameter]
	public string CompanyId { get; set; }
	#endregion
}
