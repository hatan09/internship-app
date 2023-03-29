using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class SkillTagList
{
    [Parameter]
    public List<StudentSkill> StudentSkills { get; set; }

    [Parameter]
    public List<JobSkill> JobSkills { get; set; }
}
