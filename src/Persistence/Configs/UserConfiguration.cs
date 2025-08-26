using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Persistence.Configs;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> modelBuilder)
    {
        modelBuilder
            .HasIndex(u => u.Username)
            .IsUnique(true);
        modelBuilder
            .Property(u => u.Password)
            .HasConversion(
                c => BCrypt.Net.BCrypt.HashPassword(c),
                c => c);
    }
}