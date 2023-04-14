namespace InternshipApp.Portal.AppsettingConfig;

public class JwtConfig
{
    public string JWT_Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
