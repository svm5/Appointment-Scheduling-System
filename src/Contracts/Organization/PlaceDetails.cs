namespace Contracts.Organization;

public record PlaceDetails(int Id, string Name, int Capacity, int OrganizationId, ICollection<int> Slots);