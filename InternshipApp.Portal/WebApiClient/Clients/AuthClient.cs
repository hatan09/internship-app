using InternshipApp.Api.Models;
using InternshipApp.Core.Entities;

namespace InternshipApp.Portal.WebApi;

public class AuthClient : BaseEntityHttpClient
{
    #region [ CTor ]
    public AuthClient(IHttpClientFactory clientFactory) : base(clientFactory) { }
    #endregion

    #region [ Methods - Custom ]
    public Task<LoginResponse> LoginAsync(string username, string password)
    {
        var url = $"{base._baseApiUrl}/auth/login";
        return base.PostAsJsonAsync<LoginResponse, LoginModel>(url, new()
        {
            Username = username,
            Password = password
        });
    }

    public Task RegisterAsync(Student student)
    {
        var url = $"{base._baseApiUrl}/auth/login";
        return base.PostAsJsonAsync<Student>(url, student);
    }
    #endregion
}
