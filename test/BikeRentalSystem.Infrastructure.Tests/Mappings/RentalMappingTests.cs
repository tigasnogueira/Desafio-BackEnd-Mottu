using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Mappings;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BikeRentalSystem.Infrastructure.Tests.Mappings;

public class RentalMappingTests : EntityBaseMappingTests<Rental>
{
    public RentalMappingTests()
    {
        var modelBuilder = new ModelBuilder(new ConventionSet());
        modelBuilder.Entity<Courier>(); // Ensure the Courier entity is included
        modelBuilder.Entity<Motorcycle>(); // Ensure the Motorcycle entity is included
        var entityType = modelBuilder.Entity<Rental>().Metadata;
        _builder = new EntityTypeBuilder<Rental>(entityType);
    }

    [Fact]
    public void Configure_Should_Set_Table_Name()
    {
        // Arrange
        var rentalMapping = new RentalMapping();

        // Act
        rentalMapping.Configure(_builder);

        // Assert
        _builder.Metadata.GetTableName().Should().Be("Rentals");
    }

    [Fact]
    public void Configure_Should_Set_StartDate_Property()
    {
        // Arrange
        var rentalMapping = new RentalMapping();

        // Act
        rentalMapping.Configure(_builder);
        var startDateProperty = _builder.Metadata.FindProperty(nameof(Rental.StartDate));

        // Assert
        startDateProperty.Should().NotBeNull();
        startDateProperty.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_Should_Set_EndDate_Property()
    {
        // Arrange
        var rentalMapping = new RentalMapping();

        // Act
        rentalMapping.Configure(_builder);
        var endDateProperty = _builder.Metadata.FindProperty(nameof(Rental.EndDate));

        // Assert
        endDateProperty.Should().NotBeNull();
        endDateProperty.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void Configure_Should_Set_ExpectedEndDate_Property()
    {
        // Arrange
        var rentalMapping = new RentalMapping();

        // Act
        rentalMapping.Configure(_builder);
        var expectedEndDateProperty = _builder.Metadata.FindProperty(nameof(Rental.ExpectedEndDate));

        // Assert
        expectedEndDateProperty.Should().NotBeNull();
        expectedEndDateProperty.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_Should_Set_DailyRate_Property()
    {
        // Arrange
        var rentalMapping = new RentalMapping();

        // Act
        rentalMapping.Configure(_builder);
        var dailyRateProperty = _builder.Metadata.FindProperty(nameof(Rental.DailyRate));

        // Assert
        dailyRateProperty.Should().NotBeNull();
        dailyRateProperty.IsNullable.Should().BeFalse();
        dailyRateProperty.GetColumnType().Should().Be("decimal(18,2)");
    }

    [Fact]
    public void Configure_Should_Set_Courier_ForeignKey()
    {
        // Arrange
        var rentalMapping = new RentalMapping();

        // Act
        rentalMapping.Configure(_builder);
        var courierForeignKey = _builder.Metadata.GetForeignKeys().FirstOrDefault(fk => fk.PrincipalEntityType.ClrType == typeof(Courier));

        // Assert
        courierForeignKey.Should().NotBeNull();
        courierForeignKey.DeleteBehavior.Should().Be(DeleteBehavior.Restrict);
    }

    [Fact]
    public void Configure_Should_Set_Motorcycle_ForeignKey()
    {
        // Arrange
        var rentalMapping = new RentalMapping();

        // Act
        rentalMapping.Configure(_builder);
        var motorcycleForeignKey = _builder.Metadata.GetForeignKeys().FirstOrDefault(fk => fk.PrincipalEntityType.ClrType == typeof(Motorcycle));

        // Assert
        motorcycleForeignKey.Should().NotBeNull();
        motorcycleForeignKey.DeleteBehavior.Should().Be(DeleteBehavior.Restrict);
    }
}
