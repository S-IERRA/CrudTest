using Microsoft.EntityFrameworkCore;
using Test.Data.Database.Models;

namespace Test.Data.Database;

public sealed class EntityFrameworkContext : DbContext
{
    public EntityFrameworkContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Item> Items { get; set; }
}