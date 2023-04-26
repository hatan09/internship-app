using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class ManageInternPage
{
    [Parameter]
    public string JobId { get; set; }

    [Parameter]
    public string StudentId { get; set; }
}
