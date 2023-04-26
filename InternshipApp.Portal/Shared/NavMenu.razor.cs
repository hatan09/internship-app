using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace InternshipApp.Portal.Views.Shared;

public partial class NavMenu : ComponentBase
{
	#region [ Properties ]
	public bool IsStudent { get; set; } = false;
	public bool IsTeacher { get; set; } = false;
    public bool IsRecruiter { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
    #endregion

    #region [ Properties - Inject ]
    [Inject]    
	public ILocalStorageService StorageService { get; set; }
    #endregion

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
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
            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region [ Methods - Logout ]
    public async void OnLogout()
    {
        await StorageService.RemoveItemsAsync(new List<string>() { "role", "login-student-info" });
    }
    #endregion
}
