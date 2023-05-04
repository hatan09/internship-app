using Blazored.LocalStorage;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Wave5.UI;

namespace InternshipApp.Portal.Views;

public partial class Home
{
    public bool Visible { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public User LoginUser { get; set; }
    public string Role { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Inject]
    public UserManager Users { get; private set; }

    [Inject]
    public SignInManager<User> SignInManager { get; private set; }

    #region [ Protected Override Methods - Page ]
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            try
            {
                LoginUser = new();
                var user = await LocalStorage.GetItemAsync<User>("login-user-info");
                if (user == null || string.IsNullOrWhiteSpace(user.Id))
                {
                    Visible = true;
                }
                else
                {
                    LoginUser = user;
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

    #region [ Methods - Login ]
    public async void OnLoginButtonClicked(EventArgs args)
    {
        var result = await Login();
        Visible = !result;
        StateHasChanged();
    }

    private async Task<bool> Login()
    {
        var user = await Users.FindByNameAsync(Username);
        if(user == null)
        {
            return false;
        }

        var passwordCheck = await SignInManager.CheckPasswordSignInAsync(user, Password, false);
        if (!passwordCheck.Succeeded)
        {
            return false;
        }

        var roles = await Users.GetRolesAsync(user);
        this.Role = await SaveRoleAsync(roles.ToList());

        LoginUser = user;
        await LocalStorage.SetItemAsync("login-user-info", user);
        return true;
    }

    private async Task<string> SaveRoleAsync(List<string> roles)
    {
        if (roles.Contains("admin")) {
            await this.LocalStorage.SetItemAsStringAsync("role", "ADMIN");
            return "ADMIN";
        }
        else if (roles.Contains("instructor"))  {
            await this.LocalStorage.SetItemAsStringAsync("role", "INSTRUCTOR");
            return "INSTRUCTOR";
        }
        else if (roles.Contains("recruiter")) {
            await this.LocalStorage.SetItemAsStringAsync("role", "RECRUITER");
            return "RECRUITER";
        }
        else if (roles.Contains("student")) {
            await this.LocalStorage.SetItemAsStringAsync("role", "STUDENT");
            return "STUDENT";
        }
        else {
            await this.LocalStorage.SetItemAsStringAsync("role", "GUEST");
            return "GUEST";
        }
    }
    #endregion
}
