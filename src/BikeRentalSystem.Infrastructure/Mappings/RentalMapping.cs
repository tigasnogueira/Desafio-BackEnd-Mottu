using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRentalSystem.Infrastructure.Mappings;

public class RentalMapping : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate)
            .IsRequired();

        builder.Property(r => r.Price)
            .IsRequired();

        builder.Property(r => r.IsPaid)
            .IsRequired();

        builder.Property(r => r.IsFinished)
            .IsRequired();

        builder.HasOne(r => r.Motorcycle)
            .WithMany(m => m.Rentals)
            .HasForeignKey(r => r.MotorcycleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Courier)
            .WithMany(c => c.Rentals)
            .HasForeignKey(r => r.CourierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
