using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //modelBuilder.Entity<AssetLiveStatuses>()
        //    .HasOne(a => a.Asset)
        //    .WithMany(a => a.LiveAssests)
        //    .HasForeignKey(a => a.AssetId)
        //    .IsRequired()
        //    .OnDelete(DeleteBehavior.Cascade);
            

        modelBuilder.Entity<AssetLiveStatuses>().HasKey(e => e.AssetId);
    }
}
