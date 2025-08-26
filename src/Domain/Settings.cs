namespace Domain;

public class Settings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public string AdminUsername { get; set; } = string.Empty;
    public string AdminPassword { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}