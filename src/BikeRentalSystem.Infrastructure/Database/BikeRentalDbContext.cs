using BikeRentalSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Database;

public class BikeRentalDbContext : DbContext
{
    public BikeRentalDbContext(DbContextOptions<BikeRentalDbContext> options) { }

    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach ( var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
            property.SetMaxLength(100);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BikeRentalDbContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (entry.Entity is EntityModel entity)
            {
                var now = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                    entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
