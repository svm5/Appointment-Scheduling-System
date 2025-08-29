using Contracts.Appointment;
using Contracts.Organization;
using Contracts.Security;
using Contracts.User;
using Services;
using Utils;

namespace AppointmentSchedulingSystem.Extensions;

public static class ServicesExtenstion
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IJwtUtil, JwtUtil>();
    }
}