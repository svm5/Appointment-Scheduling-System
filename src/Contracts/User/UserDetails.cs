namespace Contracts.User;

public record UserDetails(int Id, string Username, ICollection<string> Roles, int OrganizationId);