using Test.Data.Caching.Models;
using Test.Data.Database.Models;

namespace Test.Logic.Abstractions;

public interface ICachedItemService
{
    Task<List<string>?> FindNearestKey(string guess);

    Task<List<BasicItem>> FetchAllItems();
    Task<BasicItem?> FetchCachedItem(string itemName);

    Task CacheItemInfo(string itemName, BasicItem item);
    void DeleteCachedItem(string itemName);
    Task UpdateCache(Item item);
    
    Task RefreshCache(List<BasicItem> databaseItems);
}