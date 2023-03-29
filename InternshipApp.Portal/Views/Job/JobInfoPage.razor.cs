using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class JobInfoPage : ComponentBase
{
    [Parameter]
    public string JobId { get; set; }
}
