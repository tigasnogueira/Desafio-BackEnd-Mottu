using BikeRentalSystem.Api.Configuration;
using BikeRentalSystem.Infrastructure.Context;
using BikeRentalSystem.Shared.Configurations;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace BikeRentalSystem.Api.Tests.Configuration;

public class ApiSettingsTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly ApiSettings _apiSettings;

    public ApiSettingsTests()
    {
        _services = new ServiceCollection();
        _configuration = Substitute.For<IConfiguration>();
        _environment = Substitute.For<IWebHostEnvironment>();
        _apiSettings = new ApiSettings();
    }

    [Fact]
    public void ConfigureServices_ShouldAddConfigurationToServices()
    {
        // Arrange
        _environment.EnvironmentName.Returns("Development");
        Environment.SetEnvironmentVariable("APP_SECRET", "thisisasecretkey!");

        // Act
        _apiSettings.ConfigureServices(_services, _configuration, _environment);

        // Assert
        _services.Should().ContainSingle(descriptor => descriptor.ServiceType == typeof(IConfiguration));

        Environment.SetEnvironmentVariable("APP_SECRET", null);
    }

    [Fact]
    public void ConfigureServices_ShouldConfigureDatabase()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string> {
            {"DatabaseSettings:DefaultConnection", "Server=localhost;Database=testdb;User Id=testuser;Password=testpassword;"}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        Environment.SetEnvironmentVariable("APP_SECRET", "thisisasecretkey!");

        // Act
        _apiSettings.ConfigureServices(_services, configuration, _environment);

        // Assert
        _services.Should().Contain(descriptor => descriptor.ServiceType == typeof(DataContext));

        Environment.SetEnvironmentVariable("APP_SECRET", null);
    }

    [Fact]
    public void ConfigureServices_ShouldConfigureAzureBlobStorage_WhenEnvironmentVariableIsSet()
    {
        // Arrange
        Environment.SetEnvironmentVariable("AZURE_CONNECTION_STRING", "fake-connection-string");
        Environment.SetEnvironmentVariable("APP_SECRET", "thisisasecretkey!");

        var inMemorySettings = new Dictionary<string, string> {
            {"AzureBlobStorageSettings:ContainerName", "test-container"}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        _apiSettings.ConfigureServices(_services, configuration, _environment);

        // Assert
        _services.Should().ContainSingle(descriptor => descriptor.ServiceType == typeof(IConfigureOptions<AzureBlobStorageSettings>));

        Environment.SetEnvironmentVariable("AZURE_CONNECTION_STRING", null);
        Environment.SetEnvironmentVariable("APP_SECRET", null);
    }

    [Fact]
    public void ConfigureServices_ShouldFail_WhenConfigurationIsNull()
    {
        // Arrange
        IConfiguration nullConfiguration = null;

        Environment.SetEnvironmentVariable("APP_SECRET", "thisisasecretkey!");

        // Act
        Action act = () => _apiSettings.ConfigureServices(_services, nullConfiguration, _environment);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        Environment.SetEnvironmentVariable("APP_SECRET", null);
    }
}
