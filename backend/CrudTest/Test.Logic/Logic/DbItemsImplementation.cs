using Microsoft.EntityFrameworkCore;
using Test.Data.Database;
using Test.Data.Database.Models;
using Test.Logic.Abstractions;
using Test.Logic.Helpers;
using Test.Logic.Models;
using ILogger = Serilog.ILogger;

namespace Test.Logic.Logic;

public class DbItemImplementation : IDbItemService
{
    private readonly ILogger _logger;
    private readonly IDbContextFactory<EntityFrameworkContext> _dbContext;

    public DbItemImplementation(ILogger logger, IDbContextFactory<EntityFrameworkContext> dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Item> CreateItem(CreateItemRequest createItemRequest)
    {
        _logger.Information("DbItemImplementation:CreateItem - Start");

        await using var context = await _dbContext.CreateDbContextAsync();

        var newItem = (Item)createItemRequest;
        newItem.ItemCode = RandomGenerator.String(16);

        var databaseEntry = context.Items.Add(newItem);
        await context.SaveChangesAsync();

        _logger.Information("CreateItem - End");

        return databaseEntry.Entity;
    }

    public async Task<GenericResponse<Item>> ModifyItem(string itemCode, ModifyItemRequest request)
    {
        _logger.Information($"DbItemImplementation:ModifyItem - Start (itemCode: {itemCode})");

        await using var context = await _dbContext.CreateDbContextAsync();
        var item = await context.Items.FirstOrDefaultAsync(x => x.ItemCode == itemCode);

        if (item is null)
        {
            _logger.Information($"DbItemImplementation:ModifyItem - Item not found (itemCode: {itemCode})");
            return new GenericResponse<Item>(null, "Item not found");
        }

        var itemProperties = typeof(Item).GetProperties();
        var requestProperties = typeof(ModifyItemRequest).GetProperties();

        foreach (var prop in requestProperties)
        {
            var itemProp = itemProperties.FirstOrDefault(p => p.Name == prop.Name);

            if (itemProp == null) 
                continue;
            
            var requestValue = prop.GetValue(request);
            var itemValue = itemProp.GetValue(item);

            if (requestValue != null && !requestValue.Equals(itemValue))
            {
                itemProp.SetValue(item, requestValue);
            }
        }

        await context.SaveChangesAsync();

        _logger.Information($"DbItemImplementation:ModifyItem - End (itemCode: {itemCode})");

        return new GenericResponse<Item>(item, null);
    }

    public async Task<GenericResponse<Item>> DeleteItem(string itemCode)
    {
        _logger.Information($"DbItemImplementation:DeleteItem - Start (itemCode: {itemCode})");

        await using var context = await _dbContext.CreateDbContextAsync();

        if (await context.Items.FirstOrDefaultAsync(x => x.ItemCode == itemCode) is not { } item)
        {
            _logger.Information($"DbItemImplementation:DeleteItem - Item not found (itemCode: {itemCode})");
            return new GenericResponse<Item>(null, "Item not found");
        }

        context.Items.Remove(item);
        await context.SaveChangesAsync();

        _logger.Information($"DbItemImplementation:DeleteItem - End (itemCode: {itemCode})");
        
        return new GenericResponse<Item>(item, null);
    }

    public async Task<List<Item>> FetchDatabaseItems()
    {
        _logger.Information($"DbItemImplementation:FetchDatabaseItems - Start");

        await using var context = await _dbContext.CreateDbContextAsync();

        IQueryable<Item> query = context.Items.AsNoTracking();

        _logger.Information($"DbItemImplementation:FetchDatabaseItems - End");

        return await query.ToListAsync();
    }
    
    public async Task<GenericResponse<Item>> FetchDatabaseItem(string itemCode)
    {
        _logger.Information($"DbItemImplementation:FetchDatabaseItem - Start(itemCode: {itemCode})");

        await using var context = await _dbContext.CreateDbContextAsync();

        if (await context.Items.FirstOrDefaultAsync(x => x.ItemCode == itemCode) is not { } item)
        {
            _logger.Information($"DbItemImplementation:FetchDatabaseItem - Item not found (itemCode: {itemCode})");
            return new GenericResponse<Item>(null, "Item not found");
        }

        _logger.Information($"DbItemImplementation:FetchDatabaseItem - End");

        return new GenericResponse<Item>(item, null);
    }
}