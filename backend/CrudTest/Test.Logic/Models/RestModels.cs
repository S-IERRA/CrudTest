using Mapster;
using Test.Data.Database.Models;

namespace Test.Logic.Models;

[AdaptTo(typeof(Item))]
public record CreateItemRequest
{
    public required string Description { get; set; }
    public bool Active { get; set; }
    public string? CustomerDescription { get; set; }
    
    public bool SalesItem { get; set; }
    public bool StockItem { get; set; }
    public bool PurchasedItem { get; set; }
    
    public required string Barcode { get; set; }
    public int ManageItemBy { get; set; }
    
    public decimal MinimumInventory { get; set; }
    public decimal MaximumInventory { get; set; }
    
    public string? Remarks { get; set; }
    public required string ImagePath { get; set; }
    
    public static explicit operator Item(CreateItemRequest itemRequest) =>
        itemRequest.Adapt<Item>();
}


[AdaptTo(typeof(Item))]
public record ModifyItemRequest
{
    public string? Description { get; set; }
    public bool? Active { get; set; }
    public string? CustomerDescription { get; set; }
    public bool? SalesItem { get; set; }
    public bool? StockItem { get; set; }
    public bool? PurchasedItem { get; set; }
    public string? Barcode { get; set; }
    public int? ManageItemBy { get; set; }
    public decimal? MinimumInventory { get; set; }
    public decimal? MaximumInventory { get; set; }
    public string? Remarks { get; set; }
    public string? ImagePath { get; set; }
    
    public static explicit operator Item(ModifyItemRequest itemRequest) =>
        itemRequest.Adapt<Item>();
}