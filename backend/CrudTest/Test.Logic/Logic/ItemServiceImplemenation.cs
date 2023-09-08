using Test.Data.Caching.Models;
using Test.Data.Database.Models;
using Test.Logic.Abstractions;
using Test.Logic.Models;

using ILogger = Serilog.ILogger;

namespace Test.Logic.Logic;

public class ItemServiceImplementation : IItemService
{
    private readonly ILogger _logger;
    private readonly ICachedItemService _cachedItemService;
    private readonly IDbItemService _dbItemService;

    public ItemServiceImplementation(ILogger logger, ICachedItemService cachedItemService, IDbItemService dbItemService)
    {
        _logger = logger;
        _cachedItemService = cachedItemService;
        _dbItemService = dbItemService;
    }

    public async Task<List<BasicItem>> FetchAllItemsAsync()
    {
        _logger.Information("ItemServiceImplementation:FetchAllItemsAsync - Start");

        List<BasicItem> items = await _cachedItemService.FetchAllItems();
        if (items.Count is 0)
        {
            List<Item> databaseItems = await _dbItemService.FetchDatabaseItems();
            foreach (var databaseItem in databaseItems)
            {
                var basicItem = (BasicItem)databaseItem;
                
                items.Add(basicItem);
                await _cachedItemService.CacheItemInfo(databaseItem.ItemCode, basicItem);
            }
        }

        _logger.Information("ItemServiceImplementation:FetchAllItemsAsync - End");

        return items;
    }

    public async Task<Item> CreateItemAsync(CreateItemRequest request)
    {
        _logger.Information($"ItemServiceImplementation:CreateItemAsync - Start (CreateItemRequest: {request})");

        Item newItem = await _dbItemService.CreateItem(request);
        await _cachedItemService.CacheItemInfo(newItem.Description, (BasicItem)newItem);

        _logger.Information($"ItemServiceImplementation:CreateItemAsync - End");

        return newItem;
    }

    public async Task<List<BasicItem?>> SearchItemAsync(string itemName)
    {
        _logger.Information($"ItemServiceImplementation:SearchItemAsync - Start (itemName: {itemName})");

        List<string>? itemKeys = await _cachedItemService.FindNearestKey(itemName);
        if (itemKeys is null || itemKeys.Count is 0)
            return new List<BasicItem?>();
        
        var basicItems = new List<BasicItem?>();
        foreach (var itemKey in itemKeys)
            basicItems.Add(await _cachedItemService.FetchCachedItem(itemKey));
        
        if (basicItems.Count != itemKeys.Count)
        {
            _logger.Error($"ItemServiceImplementation:SearchItemAsync - Failed to fetch cached value (itemName: {itemName}");
            return new List<BasicItem?>();
        }
        
        _logger.Information($"ItemServiceImplementation:SearchItemAsync - End (itemName: {itemName})");

        return basicItems;
    }

    public async Task<GenericResponse<Item>> FetchItemDetailsAsync(string itemCode)
    {
        _logger.Information($"ItemServiceImplementation:FetchItemDetailsAsync - Start (itemCode: {itemCode})");

        GenericResponse<Item> genericResponse = await _dbItemService.FetchDatabaseItem(itemCode);
        if (genericResponse.ErrorMessage is not null)
            return genericResponse;

        _logger.Information($"ItemServiceImplementation:FetchItemDetailsAsync - End (itemCode: {itemCode})");

        return genericResponse;
    }

    public async Task<GenericResponse<Item>> UpdateItemAsync(string itemCode, ModifyItemRequest request)
    {
        _logger.Information($"ItemServiceImplementation:UpdateItemAsync - Start (itemCode: {itemCode}, ModifyItemRequest: {request})");

        GenericResponse<Item> genericResponse = await _dbItemService.ModifyItem(itemCode, request);
        if (genericResponse.ErrorMessage is not null)
            return genericResponse;

        await _cachedItemService.UpdateCache(genericResponse.Model!);
        
        _logger.Information($"ItemServiceImplementation:UpdateItemAsync - End (itemCode: {itemCode})");

        return genericResponse;
    }

    public async Task<GenericResponse<Item>> DeleteItemAsync(string itemCode)
    {
        _logger.Information($"ItemServiceImplementation:DeleteItemAsync - Start (itemCode: {itemCode})");

        GenericResponse<Item> genericResponse = await _dbItemService.DeleteItem(itemCode);
        if (genericResponse.ErrorMessage is not null)
            return genericResponse;

        _cachedItemService.DeleteCachedItem(genericResponse.Model!.Description);

        _logger.Information($"ItemServiceImplementation:DeleteItemAsync - End (itemCode: {itemCode})");

        return genericResponse;
    }

    public async Task ClearItemCache()
    {
        _logger.Information($"ItemServiceImplementation:ClearItemCache - Start");

        List<BasicItem> items = await FetchAllItemsAsync();
        await _cachedItemService.RefreshCache(items);
        
        _logger.Information($"ItemServiceImplementation:ClearItemCache - End");
    }
}