namespace InternshipApp.Portal.WebApi;

public class LoginResponse
{
    public string Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public List<string> roles { get; set; } = new();
}
