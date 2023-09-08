using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Test.Data.Database.Models;

public record Item : IEntityTypeConfiguration<Item>
{
    [Key]
    public required string ItemCode { get; set; } 
    
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
    
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("ItemMaster");
        
        builder.HasKey(e => e.ItemCode);
        builder.HasIndex(e => e.ItemCode)
            .IsUnique();
        
        builder.Property(e => e.ItemCode)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(e => e.Active)
            .IsRequired();

        builder.Property(e => e.CustomerDescription)
            .HasMaxLength(300);

        builder.Property(e => e.SalesItem)
            .IsRequired();

        builder.Property(e => e.StockItem)
            .IsRequired();

        builder.Property(e => e.PurchasedItem)
            .IsRequired();

        builder.Property(e => e.Barcode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ManageItemBy)
            .IsRequired();

        builder.Property(e => e.MinimumInventory)
            .IsRequired();

        builder.Property(e => e.MaximumInventory)
            .IsRequired();

        builder.Property(e => e.Remarks)
            .HasMaxLength(-1);

        builder.Property(e => e.ImagePath)
            .IsRequired()
            .HasMaxLength(-1);
    }
}