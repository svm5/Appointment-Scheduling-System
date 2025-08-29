using AutoMapper;
using Mapping;

namespace AppointmentSchedulingSystem.Extensions;

public static class MappingExtension
{
    public static void AddMapping(this IServiceCollection services)
    {
        List<Profile> profiles = new List<Profile>()
        {
            new OrganizationMapping(), 
            new PlaceMapping(),
            new SlotMapping(),
            new RoleMapping(),
            new UserMapping(),
            new AppointmentMapping(),
        };
        services.AddAutoMapper(c => c.AddProfiles(profiles));
    }
}