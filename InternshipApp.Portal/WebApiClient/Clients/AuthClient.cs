using Blazored.LocalStorage;
using InternshipApp.Api.Models;
using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.WebApi;

public class AuthClient : BaseEntityHttpClient
{
    #region [ CTor ]
    public AuthClient(IHttpClientFactory clientFactory, ILocalStorageService localStorageService) : base(clientFactory, localStorageService) { }
    #endregion

    #region [ Methods - Custom ]
    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var url = $"{base._baseApiUrl}auth/login";
        using var client = await base.CreateClientAsync();
        var response = await client.PostAsJsonAsync<LoginResponse, LoginModel>(url, new()
        {
            Username = username,
            Password = password,
        }, false);
        return response;
    }

    public Task RegisterAsync(Student student)
    {
        var url = $"{base._baseApiUrl}/auth/login";
        return base.PostAsJsonAsync<Student>(url, student);
    }
    #endregion
}
