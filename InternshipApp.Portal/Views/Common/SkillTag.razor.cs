﻿using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;
public partial class SkillTag
{
    [Parameter]
    public string Name { get; set; } = "";

    [Parameter]
    public string Description { get; set; } = "";

    [Parameter]
    public bool IsEditButton { get; set; } = false;

    [Parameter]
    public EventCallback OnClickEventCallBack { get; set; }

    private async void OnClicked()
    {
        await OnClickEventCallBack.InvokeAsync();
    }
}