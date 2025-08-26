using System.Text.Json.Serialization;
using Domain.User;

namespace Contracts.User;

// public record CreateUserRequest(
//     string Username, 
//     string Password, 
//     int OrganizationId,
//     [property: JsonIgnore] IList<string>? Roles);
public class CreateUserRequest
{
    public string Username { get; init; }
    public string Password { get; init; }
    public int OrganizationId { get; init; }
    
    [JsonIgnore]
    public IList<string> Roles { get; set; }

    public CreateUserRequest(string username, string password, int organizationId)
    {
        Username = username;
        Password = password;
        OrganizationId = organizationId;
        Roles = new List<string>();
    }
}