namespace Contracts.Organization;

public record CreatePlaceRequest(string Name, int Capacity, int OrganizationId);