using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Mappings;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BikeRentalSystem.Infrastructure.Tests.Mappings;

public class CourierMappingTests : EntityBaseMappingTests<Courier>
{
    public CourierMappingTests()
    {
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var entityType = modelBuilder.Entity<Courier>().Metadata;
        _builder = new EntityTypeBuilder<Courier>(entityType);
    }

    [Fact]
    public void Configure_Should_Set_Table_Name()
    {
        // Arrange
        var courierMapping = new CourierMapping();

        // Act
        courierMapping.Configure(_builder);

        // Assert
        _builder.Metadata.GetTableName().Should().Be("Couriers");
    }

    [Fact]
    public void Configure_Should_Set_Name_Property()
    {
        // Arrange
        var courierMapping = new CourierMapping();

        // Act
        courierMapping.Configure(_builder);
        var nameProperty = _builder.Metadata.FindProperty(nameof(Courier.Name));

        // Assert
        nameProperty.Should().NotBeNull();
        nameProperty.IsNullable.Should().BeFalse();
        nameProperty.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void Configure_Should_Set_Cnpj_Property_And_Index()
    {
        // Arrange
        var courierMapping = new CourierMapping();

        // Act
        courierMapping.Configure(_builder);
        var cnpjProperty = _builder.Metadata.FindProperty(nameof(Courier.Cnpj));
        var cnpjIndex = _builder.Metadata.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == nameof(Courier.Cnpj)));

        // Assert
        cnpjProperty.Should().NotBeNull();
        cnpjProperty.IsNullable.Should().BeFalse();
        cnpjProperty.GetMaxLength().Should().Be(20);
        cnpjIndex.Should().NotBeNull();
        cnpjIndex.IsUnique.Should().BeTrue();
    }

    [Fact]
    public void Configure_Should_Set_BirthDate_Property()
    {
        // Arrange
        var courierMapping = new CourierMapping();

        // Act
        courierMapping.Configure(_builder);
        var birthDateProperty = _builder.Metadata.FindProperty(nameof(Courier.BirthDate));

        // Assert
        birthDateProperty.Should().NotBeNull();
        birthDateProperty.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_Should_Set_CnhNumber_Property_And_Index()
    {
        // Arrange
        var courierMapping = new CourierMapping();

        // Act
        courierMapping.Configure(_builder);
        var cnhNumberProperty = _builder.Metadata.FindProperty(nameof(Courier.CnhNumber));
        var cnhNumberIndex = _builder.Metadata.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == nameof(Courier.CnhNumber)));

        // Assert
        cnhNumberProperty.Should().NotBeNull();
        cnhNumberProperty.IsNullable.Should().BeFalse();
        cnhNumberProperty.GetMaxLength().Should().Be(20);
        cnhNumberIndex.Should().NotBeNull();
        cnhNumberIndex.IsUnique.Should().BeTrue();
    }

    [Fact]
    public void Configure_Should_Set_CnhType_Property()
    {
        // Arrange
        var courierMapping = new CourierMapping();

        // Act
        courierMapping.Configure(_builder);
        var cnhTypeProperty = _builder.Metadata.FindProperty(nameof(Courier.CnhType));

        // Assert
        cnhTypeProperty.Should().NotBeNull();
        cnhTypeProperty.IsNullable.Should().BeFalse();
        cnhTypeProperty.GetMaxLength().Should().Be(5);
    }

    [Fact]
    public void Configure_Should_Set_CnhImage_Property()
    {
        // Arrange
        var courierMapping = new CourierMapping();

        // Act
        courierMapping.Configure(_builder);
        var cnhImageProperty = _builder.Metadata.FindProperty(nameof(Courier.CnhImage));

        // Assert
        cnhImageProperty.Should().NotBeNull();
        cnhImageProperty.IsNullable.Should().BeTrue();
        cnhImageProperty.GetMaxLength().Should().Be(255);
    }
}
