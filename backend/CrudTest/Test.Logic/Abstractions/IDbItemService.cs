using Test.Data.Database.Models;
using Test.Logic.Models;

namespace Test.Logic.Abstractions;

public interface IDbItemService
{
    Task<Item> CreateItem(CreateItemRequest createItemRequest); 
    Task<GenericResponse<Item>> ModifyItem(string itemCode, ModifyItemRequest request);
    Task<GenericResponse<Item>> DeleteItem(string itemCode);
    Task<List<Item>> FetchDatabaseItems();
    Task<GenericResponse<Item>> FetchDatabaseItem(string itemCode);
}