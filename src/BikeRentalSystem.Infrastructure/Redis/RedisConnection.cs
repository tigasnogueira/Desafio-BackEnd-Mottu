using StackExchange.Redis;

namespace BikeRentalSystem.Infrastructure.Redis;

public class RedisConnection
{
    private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
    {
        var configuration = ConfigurationOptions.Parse("localhost:6379");
        configuration.AbortOnConnectFail = false;
        return ConnectionMultiplexer.Connect(configuration);
    });

    public static ConnectionMultiplexer Connection => lazyConnection.Value;
}
