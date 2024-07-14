using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BikeRentalSystem.Infrastructure.Identity;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var apiProjectPath = Path.Combine(basePath, "BikeRentalSystem.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetSection("DatabaseSettings:DefaultConnection").Value;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("The connection string 'DefaultConnection' was not found.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
