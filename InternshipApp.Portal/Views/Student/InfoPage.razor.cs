﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views;

public partial class InfoPage
{
    #region [ Properties ]
    [Inject]
    public ILocalStorageService StorageService { get; set; }

    public bool IsStudent { get; set; } = false;
    #endregion

    [Parameter]
    public string StudentId { get; set; }

    #region [ Methods - Override ]
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            var role = await StorageService.GetItemAsStringAsync("role");
            if (role == "STUDENT") IsStudent = true;

            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion
}
