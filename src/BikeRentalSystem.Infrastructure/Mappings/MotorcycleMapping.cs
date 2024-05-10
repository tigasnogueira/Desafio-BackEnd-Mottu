using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRentalSystem.Infrastructure.Mappings;

public class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Year)
            .IsRequired();

        builder.Property(m => m.LicensePlate)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(m => m.Mileage)
            .IsRequired();

        builder.Property(m => m.EngineSize)
            .IsRequired();

        builder.Property(m => m.Color)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.ImageUrl)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.IsRented)
            .IsRequired();

        builder.HasMany(m => m.Rentals)
            .WithOne(r => r.Motorcycle)
            .HasForeignKey(r => r.MotorcycleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
