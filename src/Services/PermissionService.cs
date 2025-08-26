using Contracts.Organization;
using Contracts.User;
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

    
    public async Task<bool> CheckPlaceAccess(string username, int placeOrganizationId)
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
        
        return user.OrganizationId == placeOrganizationId;
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

    public async Task<bool> CheckCreateManagerAccess(string username, int organizationId)
    {
        User user = await GetUserAsync(username);
        var userRolesStr = GetUserRolesStr(user);
        if (userRolesStr.Contains(RoleTypes.Admin.ToString()))
        {
            return true;
        }

        if (userRolesStr.Contains(RoleTypes.SeniorManager.ToString()))
        {
            return false;
        }
        
        return user.OrganizationId == organizationId;
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
}