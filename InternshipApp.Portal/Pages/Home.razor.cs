using Blazored.LocalStorage;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Wave5.UI;

namespace InternshipApp.Portal.Views;

public partial class Home
{
    public bool Visible { get; set; }

    public bool IsRegisterOpen { get; set; } = true;
    public bool IsLogin { get; set; } = true;

    public string StudentId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }

    public int Year { get; set; }
    public double GPA { get; set; }
    public int Credits { get; set; }

    public string LoginMsg { get; set; }
    public bool IsDisable { get; set; }
    public bool IsProcessing { get; set; }

    public User LoginUser { get; set; }
    public string Role { get; set; }
    public string InternshipTitle { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime CloseRegistrationTime { get; set; }
    public DateTime SummaryTime { get; set; }
    public DateTime EndTime { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public UserManager Users { get; private set; }

    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public SignInManager<User> SignInManager { get; private set; }

    [Inject]
    public NavigationManager NavigationManager { get; private set; }

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

    private void OnStudentIdChanged(Microsoft.AspNetCore.Components.ChangeEventArgs args)
    {
        var value = args.Value.ToString();
        if (!string.IsNullOrEmpty(value))
        {
            StudentId = value;
            Username = value;
            StateHasChanged();
        }
    }
    #endregion

    #region [ Methods - Login ]
    public async void ToggleRegister()
    {
        if (!IsRegisterOpen)
        {
            IsLogin = true;
            await JSRuntime.InvokeVoidAsync("alert", "Out of registration time.");
            return;
        }
        LoginMsg = "";
        IsLogin = !IsLogin;
        StateHasChanged();
    }

    public void ToggleLogin()
    {
        IsLogin = true;
        Visible = !Visible;
        StateHasChanged();
    }

    private void OnProcess()
    {
        IsDisable = true;
        IsProcessing = true;
        StateHasChanged();
    }

    private void OnAfterProcess()
    {
        IsDisable = false;
        IsProcessing = false;
        StateHasChanged();
    }

    public void EnterEventHandler(KeyboardEventArgs args)
    {
        if (args.Code == "Enter" || args.Code == "NumpadEnter")
        {
            OnLoginButtonClicked(null);
        }
    }

    public async void OnLoginButtonClicked(EventArgs args)
    {
        OnProcess();
        var result = await Login();
        OnAfterProcess();
        Visible = !result;
        if (result)
        {
            NavigationManager.NavigateTo("/", result);
            ResetForm();
        }
    }

    public async void OnRegisterButtonClicked(EventArgs args)
    {
        var result = await Register();
        IsLogin = result;
        StateHasChanged();
    }

    private async Task<bool> Login()
    {
        var user = await Users.FindByNameAsync(Username);
        if(user == null)
        {
            LoginMsg = "Username or password is incorrect";
            return false;
        }

        var passwordCheck = await SignInManager.CheckPasswordSignInAsync(user, Password, false);
        if (!passwordCheck.Succeeded)
        {
            LoginMsg = "Username or password is incorrect";
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

    private async Task<bool> Register()
    {
        var student = new Student() { 
            FullName = this.Name,
            StudentId = this.StudentId,
            Email = this.Email,
            Year = this.Year,
            Credit = this.Credits,
            GPA = this.GPA,
            UserName = this.Username,
            Stat = Stat.WAITING
        };
        var result = await Students.CreateAsync(student);
        if(!result.Succeeded)
        {
            LoginMsg = "Can't create user";
            StateHasChanged();
            return false;
        }

        result = await Students.AddPasswordAsync(student, this.Password);
        if (!result.Succeeded)
        {
            LoginMsg = "Can't provide user with password";
            StateHasChanged();
            return false;
        }

        result = await Students.AddToRoleAsync(student, "student");
        if (!result.Succeeded)
        {
            LoginMsg = "Can't add role";
            StateHasChanged();
            return false;
        }

        await this.JSRuntime.InvokeVoidAsync("alert", "Account Created!");
        ResetForm();
        return true;
    }

    private void OnResetRegForm()
    {
        ResetForm();
        Username = "";
        StateHasChanged();
    }

    private void ResetForm()
    {
        StudentId = "";
        Name = "";
        Email = "";
        Password = "";
        Year = 0;
        GPA = 0;
        Credits = 0;
        LoginMsg = "";
    }
    #endregion
}
