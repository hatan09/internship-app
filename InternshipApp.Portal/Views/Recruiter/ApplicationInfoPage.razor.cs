using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;
public partial class ApplicationInfoPage 
{
    [Parameter]
    public string StudentId { get; set; }

    [Parameter]
    public string JobId { get; set; }
}
