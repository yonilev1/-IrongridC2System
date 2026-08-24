using Microsoft.EntityFrameworkCore;
using AssetsApi.Models;
namespace Consumer.Data;

public class AssetsDbContext : DbContext
{
    public AssetsDbContext(DbContextOptions<AssetsDbContext> options)
        : base(options) 
    { }

    public DbSet<AssetLiveStatuses> AssetLiveStatuses { get; set; }
    public DbSet<AssetsEvent> Assets { get; set; }
    public DbSet<UnitsEvent> Units { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AssetLiveStatuses>()
            .HasOne(l => l.Asset)
            .WithMany(a => a.LiveAssets)
            .HasForeignKey(l => l.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssetsEvent>()
            .HasOne(a => a.Unit)
            .WithMany(u => u.Assets)
            .HasForeignKey(a => a.UnitId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<AssetLiveStatuses>().HasKey(e => e.AssetId);
    }
}
