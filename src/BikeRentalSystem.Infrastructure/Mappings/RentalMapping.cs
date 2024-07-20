using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRentalSystem.Infrastructure.Mappings;

public class RentalMapping : EntityBaseMapping<Rental>
{
    public override void Configure(EntityTypeBuilder<Rental> builder)
    {
        base.Configure(builder);

        builder.ToTable("Rentals");

        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate);

        builder.Property(r => r.ExpectedEndDate)
            .IsRequired();

        builder.Property(r => r.DailyRate)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(r => r.Courier)
            .WithMany()
            .HasForeignKey(r => r.CourierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Motorcycle)
            .WithMany()
            .HasForeignKey(r => r.MotorcycleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
