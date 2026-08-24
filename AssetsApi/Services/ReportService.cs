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

    public async Task<IEnumerable<AssetsStatusPerUnit>?> GetAllAssetsStatusOfEveryUnit(int unitId)
    {
        var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == unitId);
        if (unit == null)
            return null;

        var assets = await _context.Assets.Where(u => u.Unit.Id == unitId).Include(u => u.LiveAssets).Include(a => a.Unit).ToListAsync();

        List<AssetsStatusPerUnit> assetsPetUnit = new List<AssetsStatusPerUnit>();

        foreach (AssetsEvent ast in assets)
        {
            
            AssetsStatusPerUnit details = new AssetsStatusPerUnit
            {
                AssetId = ast.Id,
                AssetSerial = ast.AssetSerial,
                AssetType = ast.AssetType,
                PtocessedStatus = ast.LiveAssets != null ? ast.LiveAssets.ProcessedStatus : null,
                IsVerified = ast.LiveAssets != null ? ast.LiveAssets.IsVerified :  null,
                LastUpdate = ast.LiveAssets != null ? ast.LiveAssets.LastUpdate : null
            };
            assetsPetUnit.Add(details);
        }
        return assetsPetUnit;
    }

    public async Task<IEnumerable<SummeryByUnit>> GetSummaryByUnit()
    {
        var summery = await _context.Assets.
            Include(u => u.LiveAssets).
            Include(a => a.Unit)
            .GroupBy(u => u.UnitId)
            .ToListAsync();

        List<SummeryByUnit> sumAll = new List<SummeryByUnit>();

        foreach (var sum in summery)
        {
            SummeryByUnit tempSum = new SummeryByUnit
            {
                UnitId = sum.Key,
                UnitName = _context.Units.FirstOrDefault(u => u.Id == sum.Key).UnitName,
                Sector = _context.Units.FirstOrDefault(u => u.Id == sum.Key).Sector,
                TotalAssets = _context.Assets.Count(a => a.UnitId == sum.Key),
                StableAssets = _context.Assets.Include(a => a.LiveAssets).Where(a => a.LiveAssets != null && a.UnitId == sum.Key)
                .Count(a => a.LiveAssets.ProcessedStatus == "Stable"),
                WarningAssets = _context.Assets.Include(a => a.LiveAssets).Where(a => a.LiveAssets != null && a.UnitId == sum.Key)
                .Count(a => a.LiveAssets.ProcessedStatus == "Warning"),
                UnVerifiedAssets = _context.Assets.Include(a => a.LiveAssets).Where(a => a.LiveAssets != null && a.UnitId == sum.Key)
                .Count(a => a.LiveAssets.IsVerified == false)
            };
            sumAll.Add(tempSum);
        }

        return sumAll;
    }
}
