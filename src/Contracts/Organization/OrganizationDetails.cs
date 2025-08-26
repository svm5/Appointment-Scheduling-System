namespace Contracts.Organization;

public record OrganizationDetails(int Id, string Name, ICollection<int> Places);