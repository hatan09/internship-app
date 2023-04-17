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

    public string StudentId { get; set; }

    public string Password { get; set; }

    public Student LoginStudent { get; set; }

    [Inject]
    public ILocalStorageService LocalStorage { get; set; }

    [Inject]
    public StudentManager Students { get; private set; }

    [Inject]
    public SignInManager<User> SignInManager { get; private set; }

    #region [ Protected Override Methods - Page ]
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await LocalStorage.ClearAsync();
            LoginStudent = new();
            var student = await LocalStorage.GetItemAsync<Student>("login-student-info");
            if(student == null || string.IsNullOrWhiteSpace(student.Id))
            {
                Visible = true;
            }
            else
            {
                LoginStudent = student;
            }

            await base.OnInitializedAsync();
        }
        catch (Exception ex)
        {
            
        }
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
        var student = await Students.FindByStudentId(StudentId, default);
        if(student == null)
        {
            return false;
        }

        var passwordCheck = await SignInManager.CheckPasswordSignInAsync(student, Password, false);
        if (!passwordCheck.Succeeded)
        {
            return false;
        }

        LoginStudent = student;
        await LocalStorage.SetItemAsync("login-student-info", student);
        return true;
    }
    #endregion
}
