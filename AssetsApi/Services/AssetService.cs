using AssetsApi.Models;
using Consumer.Data;
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
        if (asset != null)
        {
            AssetLiveStatuses? live = await _context.AssetLiveStatuses.FirstOrDefaultAsync(a => a.AssetId == id);
            if (live != null)
                asset.LiveAssets = live;
        }
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
}
