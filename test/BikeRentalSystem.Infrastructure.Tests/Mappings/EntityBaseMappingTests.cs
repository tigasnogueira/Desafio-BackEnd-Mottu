using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Mappings;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BikeRentalSystem.Infrastructure.Tests.Mappings;

public abstract class EntityBaseMappingTests<TEntity> where TEntity : EntityBase
{
    protected EntityTypeBuilder<TEntity> _builder;

    public EntityBaseMappingTests()
    {
        var modelBuilder = new ModelBuilder(new ConventionSet());
        var entityType = modelBuilder.Entity<TEntity>().Metadata;
        _builder = new EntityTypeBuilder<TEntity>(entityType);
    }

    [Fact]
    public void Configure_Should_Set_Id_As_PrimaryKey()
    {
        // Arrange
        var entityBaseMapping = new TestEntityBaseMapping<TEntity>();

        // Act
        entityBaseMapping.Configure(_builder);

        // Assert
        _builder.Metadata.FindPrimaryKey().Properties[0].Name.Should().Be("Id");
    }

    [Fact]
    public void Configure_Should_Set_CreatedAt_Property()
    {
        // Arrange
        var entityBaseMapping = new TestEntityBaseMapping<TEntity>();

        // Act
        entityBaseMapping.Configure(_builder);
        var createdAtProperty = _builder.Metadata.FindProperty(nameof(EntityBase.CreatedAt));

        // Assert
        createdAtProperty.Should().NotBeNull();
        createdAtProperty.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void Configure_Should_Set_CreatedByUser_Property()
    {
        // Arrange
        var entityBaseMapping = new TestEntityBaseMapping<TEntity>();

        // Act
        entityBaseMapping.Configure(_builder);
        var createdByUserProperty = _builder.Metadata.FindProperty(nameof(EntityBase.CreatedByUser));

        // Assert
        createdByUserProperty.Should().NotBeNull();
        createdByUserProperty.IsNullable.Should().BeFalse();
        createdByUserProperty.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void Configure_Should_Set_UpdatedAt_Property()
    {
        // Arrange
        var entityBaseMapping = new TestEntityBaseMapping<TEntity>();

        // Act
        entityBaseMapping.Configure(_builder);
        var updatedAtProperty = _builder.Metadata.FindProperty(nameof(EntityBase.UpdatedAt));

        // Assert
        updatedAtProperty.Should().NotBeNull();
        updatedAtProperty.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void Configure_Should_Set_UpdatedByUser_Property()
    {
        // Arrange
        var entityBaseMapping = new TestEntityBaseMapping<TEntity>();

        // Act
        entityBaseMapping.Configure(_builder);
        var updatedByUserProperty = _builder.Metadata.FindProperty(nameof(EntityBase.UpdatedByUser));

        // Assert
        updatedByUserProperty.Should().NotBeNull();
        updatedByUserProperty.IsNullable.Should().BeTrue();
        updatedByUserProperty.GetMaxLength().Should().Be(100);
    }

    [Fact]
    public void Configure_Should_Set_IsDeleted_Property()
    {
        // Arrange
        var entityBaseMapping = new TestEntityBaseMapping<TEntity>();

        // Act
        entityBaseMapping.Configure(_builder);
        var isDeletedProperty = _builder.Metadata.FindProperty(nameof(EntityBase.IsDeleted));

        // Assert
        isDeletedProperty.Should().NotBeNull();
        isDeletedProperty.IsNullable.Should().BeFalse();
    }
}

public class TestEntityBaseMapping<TEntity> : EntityBaseMapping<TEntity> where TEntity : EntityBase
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);
    }
}
