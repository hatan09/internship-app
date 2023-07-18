using Blazored.LocalStorage;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views.Shared;

public partial class NavMenu : ComponentBase
{
	#region [ Properties ]
	public bool IsStudent { get; set; } = false;
	public bool IsTeacher { get; set; } = false;
    public bool IsRecruiter { get; set; } = false;
    public bool IsAdmin { get; set; } = false;

    public string UserId { get; set; } = string.Empty;
    #endregion

    #region [ Properties - Inject ]
    [Inject]    
	public ILocalStorageService StorageService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                var role = await StorageService.GetItemAsync<string>("role");
                if (!string.IsNullOrEmpty(role))
                {
                    if (role.ToUpper().Equals("STUDENT")) IsStudent = true;
                    if (role.ToUpper().Equals("INSTRUCTOR")) IsTeacher = true;
                    if (role.ToUpper().Equals("RECRUITER")) IsRecruiter = true;
                    if (role.ToUpper().Equals("ADMIN")) IsAdmin = true;

                }

                var user = await StorageService.GetItemAsync<User>("login-user-info");
                if (user != null)
                {
                    UserId = user.Id;
                }
                StateHasChanged();
                await base.OnInitializedAsync();
            }
            catch (Exception ex)
            {
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion



    #region [ Methods - Logout ]
    public async void OnLogout()
    {
        await StorageService.RemoveItemsAsync(new List<string>() { "role", "login-user-info", "accessToken" });
        NavigationManager.NavigateTo("/", true);
        StateHasChanged();
    }
    #endregion
}
