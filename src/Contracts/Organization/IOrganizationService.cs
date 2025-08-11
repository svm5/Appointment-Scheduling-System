namespace Contracts.Organization;

public interface IOrganizationService
{
    Task<OrganizationDetails> CreateOrganizationAsync(CreateOrganizationRequest createOrganizationRequest, CancellationToken cancellationToken);
    Task<OrganizationDetails> GetOrganizationByIdAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<OrganizationDetails>> GetAllOrganizationsAsync(CancellationToken cancellationToken);
    Task DeleteOrganizationByIdAsync(int id, CancellationToken cancellationToken);
}