using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Test.Data.Caching.Models;
using Test.Data.Database.Models;
using Test.Logic.Abstractions;
using Test.Logic.Constants;
using Test.Logic.Logic;
using Test.Logic.Models;
using ILogger = Serilog.ILogger;

namespace Test.Rest.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/items")]
[EnableRateLimiting("Api")]
public class ItemsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IItemService _itemService;

    public ItemsController(ILogger logger, IItemService itemService)
    {
        _logger = logger;
        _itemService = itemService;
    }

    [HttpGet("")]
    public async Task<IActionResult> FetchItems()
    {
        List<BasicItem> items = await _itemService.FetchAllItemsAsync();

        return Ok(JsonSerializer.Serialize(items, JsonHelper.JsonSerializerOptions));
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateItem(CreateItemRequest request)
    {
        Item item = await _itemService.CreateItemAsync(request);

        return Ok(JsonSerializer.Serialize(item, JsonHelper.JsonSerializerOptions));
    }

    [HttpGet("search/{itemName}")]
    public async Task<IActionResult> SearchItemAsync(string itemName)
    {
        List<BasicItem?> items = await _itemService.SearchItemAsync(itemName);

        if (items.Count != 0)
            return Ok(JsonSerializer.Serialize(items, JsonHelper.JsonSerializerOptions));
        
        _logger.Error($"SearchItemAsync - Failed to fetch cached value (itemName: {itemName})");
        return Problem(RestErrors.FailedToFetchCachedValue);
    }
    
    [HttpGet("fetch/{itemCode}")]
    public async Task<IActionResult> FetchItemDetails(string itemCode)
    {
        GenericResponse<Item> genericResponse = await _itemService.FetchItemDetailsAsync(itemCode);

        return genericResponse.ErrorMessage is not null
            ? BadRequest(new GenericResponse(genericResponse.ErrorMessage))
            : Ok(JsonSerializer.Serialize(genericResponse.Model, JsonHelper.JsonSerializerOptions));
    }
    
    [HttpPatch("{itemCode}/update")]
    public async Task<IActionResult> UpdateItem(string itemCode, ModifyItemRequest request)
    {
        GenericResponse<Item> genericResponse = await _itemService.UpdateItemAsync(itemCode, request);

        return genericResponse.ErrorMessage is not null
            ? BadRequest(new GenericResponse(genericResponse.ErrorMessage))
            : Ok();
    }
    
    [HttpDelete("{itemCode}/delete")]
    public async Task<IActionResult> DeleteItem(string itemCode)
    {
        GenericResponse<Item> genericResponse = await _itemService.DeleteItemAsync(itemCode);

        return genericResponse.ErrorMessage is not null
            ? BadRequest(new GenericResponse(genericResponse.ErrorMessage))
            : Ok();
    }

    [HttpDelete("refresh-cache")]
    public async Task<IActionResult> RefreshCache()
    {
        await _itemService.ClearItemCache();
        
        return Ok();
    }
}