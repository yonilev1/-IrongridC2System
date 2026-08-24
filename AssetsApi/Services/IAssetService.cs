using AssetsApi.Models;
namespace AssetsApi.Services;

public interface IAssetService
{
    Task<AssetsEvent?> GetAssetById(int id);
}
