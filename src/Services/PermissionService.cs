using Contracts.Organization;
using Contracts.User;
using Domain;
using Domain.Appointment;
using Domain.User;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services;

public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _context;

    public PermissionService(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<int> GetUserId(string username)
    {
        User user = await GetUserAsync(username);
        return user.Id;
    }

    public async Task<bool> CheckPlaceAccess(string username, int placeId)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return true;
        }

        if (!userRolesStr.Contains(RoleTypes.SeniorManager.ToString()))
        {
            return false;
        }
        
        Place place = await GetPlaceAsync(placeId);
        return user.OrganizationId == place.OrganizationId;
    }

    public async Task<GetPlacesFilters> GetPlacesFilters(string username)
    {
        User user = await GetUserAsync(username);
        
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return new GetPlacesFilters(null);
        }

        return new GetPlacesFilters(user.OrganizationId);
    }

    public async Task<bool> HasUserRole(string username)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        
        return userRolesStr.Contains(RoleTypes.User.ToString());
    }

    public async Task<bool> CheckCreateManagerAccess(string username, int organizationId)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return true;
        }
        
        if (!userRolesStr.Contains(RoleTypes.SeniorManager.ToString()))
        {
            return false;
        }
        
        return user.OrganizationId == organizationId;
    }

    public async Task<bool> CheckAdminOrManagerInOrganization(string username, int placeId)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return true;
        }
        
        if (!(userRolesStr.Contains(RoleTypes.SeniorManager.ToString()) 
              || userRolesStr.Contains(RoleTypes.SeniorManager.ToString())))
        {
            return false;
        }
        
        Place place = await GetPlaceAsync(placeId);
        return user.OrganizationId == place.OrganizationId;
    }

    public async Task<bool> CheckAdminOrManagerInOrganizationBySlotId(string username, int slotId)
    {
        User user = await GetUserAsync(username);
        
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return true;
        }
        
        if (!(userRolesStr.Contains(RoleTypes.SeniorManager.ToString()) 
              || userRolesStr.Contains(RoleTypes.SeniorManager.ToString())))
        {
            return false;
        }
        
        Slot slot = await GetSlotAsync(slotId);
        return slot.Place.OrganizationId == user.OrganizationId;
    }

    public async Task<bool> CheckSlotPermissionAccess(string username, int slotId)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return true;
        }
        
        Slot slot = await GetSlotAsync(slotId);
        
        return user.OrganizationId == slot.Place.OrganizationId;
    }

    public async Task<int?> GetOrganizationId(string username)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return null;
        }
        
        return user.OrganizationId;
    }

    public async Task<bool> CheckAppointmentPermissionAccess(string username, int appointmentId)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return true;
        }
        
        Appointment appointment = await GetAppointmentAsync(appointmentId);
        
        return user.OrganizationId == appointment.Slot.Place.OrganizationId;
    }

    private async Task<User> GetUserAsync(string username)
    {
        User? user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new NotFoundException($"User with username {username} not found.");
        }

        return user;
    }
    
    private List<string> GetUserRolesStr(User user)
    {
        return user.Roles.Select(r => r.Name).ToList();
    }

    private async Task<Place> GetPlaceAsync(int placeId)
    {
        Place? place = await _context.Places
            .FirstOrDefaultAsync(p => p.Id == placeId);
        if (place == null)
        {
            throw new NotFoundException($"Place with id {placeId} not found.");
        }
        
        return place;
    }

    private async Task<Slot> GetSlotAsync(int slotId)
    {
        Slot? slot = await _context.Slots
            .Include(s => s.Place)
            .FirstOrDefaultAsync(s => s.Id == slotId);
        if (slot == null)
        {
            throw new NotFoundException($"Slot with id {slotId} not found.");
        }
        
        return slot;
    }
    
    private async Task<Appointment> GetAppointmentAsync(int appointmentId)
    {
        Appointment? appointment = await _context.Appointments
            .Include(a => a.Slot)
            .ThenInclude(s => s.Place)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with id {appointmentId} not found.");
        }
        
        return appointment;
    }
}
