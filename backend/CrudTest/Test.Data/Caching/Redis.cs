using StackExchange.Redis;

namespace Test.Data.Caching;

public class RedisConnectionManager
{
    private readonly Lazy<ConnectionMultiplexer> lazyConnection;

    public RedisConnectionManager(string connectionString)
    {
        lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
    }

    public ConnectionMultiplexer Connection => lazyConnection.Value;
}