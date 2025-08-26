using Contracts.Organization;

namespace Contracts.User;

public interface IPermissionService
{
    Task<bool> CheckPlaceAccess(string username, int placeOrganizationId);
    Task<GetPlacesFilters> GetPlacesFilters(string username);
    Task<bool> CheckCreateManagerAccess(string username, int organizationId);
}