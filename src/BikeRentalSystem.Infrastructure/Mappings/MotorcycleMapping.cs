using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRentalSystem.Infrastructure.Mappings;

public class MotorcycleMapping : EntityBaseMapping<Motorcycle>
{
    public override void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        base.Configure(builder);

        builder.ToTable("Motorcycles");

        builder.Property(m => m.Year)
            .IsRequired();

        builder.Property(m => m.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Plate)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(m => m.Plate)
            .IsUnique();

        builder.HasOne(m => m.MotorcycleNotification)
            .WithOne(n => n.Motorcycle)
            .HasForeignKey<MotorcycleNotification>(n => n.MotorcycleId)
            .IsRequired(false);
    }
}
