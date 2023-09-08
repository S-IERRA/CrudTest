using Test.Data.Caching.Models;
using Test.Data.Database.Models;
using Test.Logic.Models;

namespace Test.Logic.Abstractions;

public interface IItemService
{
    Task<List<BasicItem>> FetchAllItemsAsync();
    Task<Item> CreateItemAsync(CreateItemRequest request);
    Task<List<BasicItem?>> SearchItemAsync(string itemName);
    Task<GenericResponse<Item>> FetchItemDetailsAsync(string itemCode);
    Task<GenericResponse<Item>> UpdateItemAsync(string itemCode, ModifyItemRequest request);
    Task<GenericResponse<Item>> DeleteItemAsync(string itemCode);

    Task ClearItemCache();
}