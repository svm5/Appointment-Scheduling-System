using System.Reflection;
using Domain;
using Domain.Appointment;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Slot> Slots { get; set; }
    
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Place> Places { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }   
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}