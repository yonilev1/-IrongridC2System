using AssetsApi.Dtos;
using AssetsApi.Models;
using Consumer.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace AssetsApi.Services;

public class ReportService : IReportService
{
    private readonly AssetsDbContext _context;

    public ReportService(AssetsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AssetWithStatus>> GetCriticalAssets()
    {
        var assets = await _context.Assets.
            Where(a => a.LiveAssets != null && (a.LiveAssets.ProcessedStatus == "Warning" || a.LiveAssets.IsVerified == false)).
            Include(a => a.LiveAssets).Include(a => a.Unit).ToListAsync();

        List<AssetWithStatus> assetsWithStatus = new List<AssetWithStatus>();

        foreach(AssetsEvent asst in assets)
        {
            AssetWithStatus ast = new AssetWithStatus
            {
                Id = asst.Id,
                AssetSerial = asst.AssetSerial,
                AssetType = asst.AssetType,
                UnitName = asst.Unit.UnitName,
                Sector = asst.Unit.Sector,
                PtocessedStatus = asst.LiveAssets.ProcessedStatus,
                IsVerified = asst.LiveAssets.IsVerified,
                LastUpdate = asst.LiveAssets.LastUpdate
            };
            assetsWithStatus.Add(ast);
        }
        return assetsWithStatus;
    }
}
