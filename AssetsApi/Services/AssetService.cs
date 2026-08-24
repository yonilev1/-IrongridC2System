using AssetsApi.Dtos;
using AssetsApi.Models;
using Consumer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetsApi.Services;

public class AssetService : IAssetService
{
    private readonly AssetsDbContext _context;

    public AssetService(AssetsDbContext context)
    {
        _context = context;
    }

    public async Task<AssetsEvent?> GetAssetById(int id)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        //if (asset != null)
        //{
        //    AssetLiveStatuses? live = await _context.AssetLiveStatuses.FirstOrDefaultAsync(a => a.AssetId == id);
        //    if (live != null)
        //        asset.LiveAssets = live;
        //}
        return asset;
    }

    public async Task<bool> CreateUnit(UnitsEvent unit)
    {
        if (await _context.Units.FirstOrDefaultAsync(a => a.Id == unit.Id) != null)
            return false;

        await _context.Units.AddAsync(unit);
        await _context.SaveChangesAsync();

        if (await _context.Units.FirstOrDefaultAsync(a => a.Id == unit.Id) == null)
            return false;
        return true;
    }

    public async Task<bool> UpdateAsset(int id, UpdateAsset asset)
    {
        var fullAsset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        if (fullAsset == null)
            return false;

        fullAsset.AssetSerial = asset.AssetSerial;
        fullAsset.AssetType = asset.AssetType;
        fullAsset.UnitId = asset.UnitId;
        await _context.SaveChangesAsync();
        Console.WriteLine(_context.ChangeTracker.HasChanges());
        return _context.ChangeTracker.HasChanges();

    }

    public async Task<bool> DeleteAsset(int id)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        if (asset == null)
            return false;

        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AssetsEvent>> GetAllWithStatus()
    {
        var query = await _context.Assets.ToListAsync();

        foreach (AssetsEvent asst in query)
        {
            AssetLiveStatuses? live = await _context.AssetLiveStatuses.FirstOrDefaultAsync(a => a.AssetId == asst.Id);
            if (live != null)
                asst.LiveAssets = live;
        }
        return query;
    }

    public async Task<AssetsEvent?> GetFullAssetWithStatus(int id)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        if (asset != null)
        {
            AssetLiveStatuses? live = await _context.AssetLiveStatuses.FirstOrDefaultAsync(a => a.AssetId == id);
            if (live != null)
                asset.LiveAssets = live;
        }
        return asset;
    }


    public async Task<IEnumerable<AssetsEvent>> GetAssetByStatus(string status)
    {
        var assets = await _context.Assets.Where(a => a.LiveAssets.ProcessedStatus == status).ToListAsync();
        foreach(AssetsEvent asst in assets)
        {
            AssetLiveStatuses? live = await _context.AssetLiveStatuses.FirstOrDefaultAsync(a => a.AssetId == asst.Id);
            if (live != null)
                asst.LiveAssets = live;
        }
        return assets;
    }
}
