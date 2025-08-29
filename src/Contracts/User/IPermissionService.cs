using Contracts.Organization;

namespace Contracts.User;

public interface IPermissionService
{
    Task<GetPlacesFilters> GetPlacesFilters(string username);
    Task<bool> HasUserRole(string username);
    Task<bool> CheckCreateManagerAccess(string username, int organizationId);
    Task<bool> CheckAdminOrManagerInOrganization(string username, int placeId);
    Task<bool> CheckAdminOrManagerInOrganizationBySlotId(string username, int slotId);
    
    Task<int?> GetOrganizationId(string username);
    Task<int> GetUserId(string username);
    
    Task<bool> CheckPlaceAccess(string username, int placeId);
    Task<bool> CheckSlotPermissionAccess(string username, int slotId);
    Task<bool> CheckAppointmentPermissionAccess(string username, int appointmentId);
}