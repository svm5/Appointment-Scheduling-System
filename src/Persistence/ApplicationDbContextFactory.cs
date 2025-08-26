using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

using Domain;

namespace Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private readonly IOptions<Settings> _settings;

    public ApplicationDbContextFactory(IOptions<Settings> settings)
    {
        _settings = settings;
    }
    
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(_settings.Value.ConnectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
