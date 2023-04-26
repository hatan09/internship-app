using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;
public partial class SkillTag
{
    [Parameter]
    public string Name { get; set; } = "";

    [Parameter]
    public string Description { get; set; } = "";
}