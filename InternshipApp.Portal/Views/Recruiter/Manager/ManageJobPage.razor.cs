using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class ManageJobPage
{
    [Parameter]
    public string? JobId { get; set; }
}