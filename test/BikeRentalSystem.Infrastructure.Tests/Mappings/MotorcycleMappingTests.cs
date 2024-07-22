using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Mappings;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BikeRentalSystem.Infrastructure.Tests.Mappings;

public class MotorcycleMappingTests : EntityBaseMappingTests<Motorcycle>
{
    public MotorcycleMappingTests()
    {
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var entityType = modelBuilder.Entity<Motorcycle>().Metadata;
        _builder = new EntityTypeBuilder<Motorcycle>(entityType);
    }

    [Fact]
    public void Configure_Should_Set_Table_Name()
    {
        // Arrange
        var motorcycleMapping = new MotorcycleMapping();

        // Act
        motorcycleMapping.Configure(_builder);

        // Assert
        _builder.Metadata.GetTableName().Should().Be("Motorcycles");
    }

    [Fact]
    public void Configure_Should_Set_Year_Property()
    {
        // Arrange
        var motorcycleMapping = new MotorcycleMapping();

        // Act
        motorcycleMapping.Configure(_builder);
        var yearProperty = _builder.Metadata.FindProperty(nameof(Motorcycle.Year));

        // Assert
        yearProperty.Should().NotBeNull();
        yearProperty.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_Should_Set_Model_Property()
    {
        // Arrange
        var motorcycleMapping = new MotorcycleMapping();

        // Act
        motorcycleMapping.Configure(_builder);
        var modelProperty = _builder.Metadata.FindProperty(nameof(Motorcycle.Model));

        // Assert
        modelProperty.Should().NotBeNull();
        modelProperty.IsNullable.Should().BeFalse();
        modelProperty.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void Configure_Should_Set_Plate_Property_And_Index()
    {
        // Arrange
        var motorcycleMapping = new MotorcycleMapping();

        // Act
        motorcycleMapping.Configure(_builder);
        var plateProperty = _builder.Metadata.FindProperty(nameof(Motorcycle.Plate));
        var plateIndex = _builder.Metadata.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == nameof(Motorcycle.Plate)));

        // Assert
        plateProperty.Should().NotBeNull();
        plateProperty.IsNullable.Should().BeFalse();
        plateProperty.GetMaxLength().Should().Be(10);
        plateIndex.Should().NotBeNull();
        plateIndex.IsUnique.Should().BeTrue();
    }
}
