using AssetsApi.Dtos;

namespace AssetsApi.Services;

public interface IReportService
{
    Task<IEnumerable<AssetWithStatus>> GetCriticalAssets();
}
