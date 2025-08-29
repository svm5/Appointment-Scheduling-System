using Microsoft.EntityFrameworkCore;
using Persistence;

namespace AppointmentSchedulingSystem.Extensions;

public static class DbContextFactoryExtension
{
    public static void AddDbContextFactory(this IServiceCollection services)
    {
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=6538;Database=postgres;Username=postgres;Password=postgres")
        );
    }
}