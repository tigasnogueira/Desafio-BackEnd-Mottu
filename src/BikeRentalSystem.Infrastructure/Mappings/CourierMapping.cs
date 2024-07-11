using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeRentalSystem.Infrastructure.Mappings;

public class CourierMapping : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.ToTable("Couriers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Cnpj)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.Cnpj)
            .IsUnique();

        builder.Property(c => c.BirthDate)
            .IsRequired();

        builder.Property(c => c.CnhNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.CnhNumber)
            .IsUnique();

        builder.Property(c => c.CnhType)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(c => c.CnhImage)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.IsDeleted)
            .IsRequired();
    }
}
