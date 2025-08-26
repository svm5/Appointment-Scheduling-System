using Domain.Appointment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configs;

public class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> modelBuilder)
    {
        modelBuilder // .ToTable("Slots")
            .HasOne(s => s.Appointment)
            .WithOne(a => a.Slot)
            .HasForeignKey<Appointment>(a => a.SlotId)
            .IsRequired();
    }
}