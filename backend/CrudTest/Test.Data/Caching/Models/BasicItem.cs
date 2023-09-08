using Mapster;
using Test.Data.Database.Models;

namespace Test.Data.Caching.Models;

[AdaptFrom(typeof(Item))]
public record BasicItem
{
    public required string ItemCode { get; set; }
    
    public required string Description { get; set; }
    public required string ImagePath { get; set; }
    
    public static explicit operator BasicItem(Item item) =>
        item.Adapt<BasicItem>();
}