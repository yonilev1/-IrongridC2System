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
}
