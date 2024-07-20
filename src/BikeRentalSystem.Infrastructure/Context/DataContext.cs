using BikeRentalSystem.Core.Models;
using BikeRentalSystem.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalSystem.Infrastructure.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MotorcycleMapping());
        modelBuilder.ApplyConfiguration(new CourierMapping());
        modelBuilder.ApplyConfiguration(new RentalMapping());
    }
}
