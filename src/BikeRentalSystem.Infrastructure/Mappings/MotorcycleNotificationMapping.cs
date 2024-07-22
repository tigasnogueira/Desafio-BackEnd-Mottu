using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRentalSystem.Infrastructure.Mappings;

public class MotorcycleNotificationMapping : EntityBaseMapping<MotorcycleNotification>
{
    public override void Configure(EntityTypeBuilder<MotorcycleNotification> builder)
    {
        base.Configure(builder);

        builder.ToTable("MotorcycleNotifications");

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(n => n.Motorcycle)
            .WithOne(m => m.MotorcycleNotification)
            .HasForeignKey<MotorcycleNotification>(n => n.MotorcycleId);
    }
}
