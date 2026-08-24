using AssetsApi.Dtos;
using AssetsApi.Models;
using Consumer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AssetsApi.Services;

public class AssetService : IAssetService
{
    private readonly AssetsDbContext _context;
    private readonly IDatabase _redis;

    public AssetService(IConnectionMultiplexer muxer, AssetsDbContext context)
    {
        _context = context;
        _redis = muxer.GetDatabase();
    }

    public async Task<AssetsEvent?> GetAssetById(int id)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
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
        _context.Assets.Update(fullAsset);
        await _context.SaveChangesAsync();
        return true;

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
        var query = await _context.Assets.Include(a => a.LiveAssets).ToListAsync();
        return query;
    }

    public async Task<AssetsEvent?> GetFullAssetWithStatus(int id)
    {
        string? json;
        var key = id.ToString();
        Console.WriteLine($"Key: {key}");
        json = await _redis.StringGetAsync(key);
        Console.WriteLine($"this is json {json}");
        if (string.IsNullOrEmpty(json) ||json == null)
        {
            var query = _context.Assets.Include(a => a.LiveAssets).AsQueryable();
            var asset = await query.FirstOrDefaultAsync(a => a.Id == id);
            if (asset != null)
            {
                Console.WriteLine(JsonSerializer.Serialize(asset));
                var setTask = _redis.StringSetAsync(key, JsonSerializer.Serialize(asset));
                var expireTask = _redis.KeyExpireAsync(key, TimeSpan.FromMinutes(5));
                await Task.WhenAll(setTask, expireTask);
            }

            return asset;   
        }
        var asst = JsonSerializer.Deserialize<AssetsEvent>(json);
        return asst;
    }


    public async Task<IEnumerable<AssetsEvent>> GetAssetByStatus(string status)
    {
        var assets = await _context.Assets.Where(a => a.LiveAssets.ProcessedStatus == status).Include(a => a.LiveAssets).ToListAsync();
        //foreach(AssetsEvent asst in assets)
        //{
        //    AssetLiveStatuses? live = await _context.AssetLiveStatuses.FirstOrDefaultAsync(a => a.AssetId == asst.Id);
        //    if (live != null)
        //        asst.LiveAssets = live;
        //}
        return assets;
    }


}
