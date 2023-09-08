using Test.Data.Caching.Models;
using Test.Logic.Abstractions;

namespace Test.Logic.HangfireJobs;

public class RefreshCacheJob
{
    private readonly IItemService _itemService;
    private readonly ICachedItemService _cachedItemService;
    
    public RefreshCacheJob(IItemService itemService, ICachedItemService cachedItemService)
    {
        _itemService = itemService;
        _cachedItemService = cachedItemService;
    }

    public async Task RefreshCache()
    {
        List<BasicItem> items = await _itemService.FetchAllItemsAsync();
        await _cachedItemService.RefreshCache(items);
    }
}