using Microsoft.EntityFrameworkCore;
using Consumer.Models;
namespace Consumer.Data;

public class AssetsDbContext : DbContext
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options)
        : base(options) 
    { }

    public DbSet<AssetLiveStatuses> AssetLiveStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AssetLiveStatuses>().HasKey(e => e.AssetId);
    }
}
