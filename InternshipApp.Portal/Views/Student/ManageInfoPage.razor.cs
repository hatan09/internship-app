using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class ManageInfoPage
{
    [Parameter]
    public string StudentId { get; set; }
}
