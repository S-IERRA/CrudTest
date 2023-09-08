using System.Text.Json;
using StackExchange.Redis;
using Test.Data.Caching;
using Test.Data.Caching.Models;
using Test.Data.Database.Models;
using Test.Logic.Abstractions;
using ILogger = Serilog.ILogger;

namespace Test.Logic.Logic;

public class CachedItemImplementation : ICachedItemService
{
    private readonly ConnectionMultiplexer _cacheClient; // Store the ConnectionMultiplexer
    private readonly IDatabase _cacheDb;
    private readonly ILogger _logger;

    private const string CacheKey = "Items:";

    public CachedItemImplementation(RedisConnectionManager redisConnectionManager, ILogger logger)
    {
        _cacheDb = redisConnectionManager.Connection.GetDatabase();
        _cacheClient = redisConnectionManager.Connection; 

        _logger = logger;
    }
    
    private IEnumerable<RedisKey> GetKeysWithPrefix()
    {
        var server = _cacheClient.GetServer(_cacheClient.GetEndPoints()[0]);
        return server.Keys(pattern: CacheKey + "*");
    }

    public async Task CacheItemInfo(string itemName, BasicItem item)
    {
        string key = $"{CacheKey}{itemName}";
        string json = JsonSerializer.Serialize(item, JsonHelper.JsonSerializerOptions);

        await _cacheDb.StringSetAsync(key, json);
    }

    public async Task<BasicItem?> FetchCachedItem(string itemName)
    {
        string key = $"{CacheKey}{itemName}";
        RedisValue cachedData = await _cacheDb.StringGetAsync(key);
        if (cachedData.IsNull)
            return null;

        JsonHelper.TryDeserialize<BasicItem>(cachedData.ToString(), out var deserialized);
        if(deserialized is null)
            _logger.Error("CachedItemImplementation:FetchCachedItem Failed to deserialize {message} to <{class}>", cachedData.ToString(), typeof(BasicItem));

        return deserialized;
    }

    public void DeleteCachedItem(string itemName)
    {
        string key = $"{CacheKey}{itemName}";
        _cacheDb.KeyDelete(key);
    }

    public async Task<List<BasicItem>> FetchAllItems()
    {
        var basicItems = new List<BasicItem>();
        IEnumerable<RedisKey> keys = GetKeysWithPrefix();

        foreach (var key in keys)
        {
            RedisValue cachedData = await _cacheDb.StringGetAsync(key);
            if (cachedData.IsNull)
                continue;

            JsonHelper.TryDeserialize<BasicItem>(cachedData.ToString(), out var deserialized);
            if (deserialized is null)
            {
                _logger.Error("CachedItemImplementation:FetchAllItems Failed to deserialize {message} to <{class}>", cachedData.ToString(), typeof(BasicItem));
                continue;
            }

            basicItems.Add(deserialized);
        }

        return basicItems;
    }

    public async Task<List<string>?> FindNearestKey(string guess)
    {
        IEnumerable<RedisKey> keys = GetKeysWithPrefix();

        List<string>? nearestKeys = new List<string>();

        foreach (var key in keys)
        {
            string existingKey = key.ToString()[CacheKey.Length..];
            if (existingKey.Contains(guess, StringComparison.OrdinalIgnoreCase))
                nearestKeys.Add(existingKey);
        }

        return nearestKeys;
    }

    public async Task UpdateCache(Item item)
    {
        DeleteCachedItem(item.Description);
        await CacheItemInfo(item.Description, (BasicItem)item);
    }

    private async Task ClearItemCache()
    {
        _logger.Information("Clearing item cache: Started");

        IEnumerable<RedisKey> keys = GetKeysWithPrefix();
        var tasks = new List<Task>();

        foreach (var key in keys)
            tasks.Add(_cacheDb.KeyDeleteAsync(key));

        await Task.WhenAll(tasks);

        _logger.Information("Clearing item cache: Completed");
    }

    public async Task RefreshCache(List<BasicItem> databaseItems)
    {
        await ClearItemCache();
        
        foreach (var databaseItem in databaseItems)
            await CacheItemInfo(databaseItem.Description, databaseItem);
    }
}