using AssetsApi.Dtos;
using AssetsApi.Models;
using StackExchange.Redis;
namespace AssetsApi.Services;

public interface IAssetService
{
    Task<AssetsEvent?> GetAssetById(int id);
    Task<bool> CreateUnit(UnitsEvent unit);
    Task<bool> UpdateAsset(int id, UpdateAsset asset);
    Task<bool> DeleteAsset(int id);
    Task<IEnumerable<AssetsEvent>> GetAllWithStatus();
    Task<AssetsEvent?> GetFullAssetWithStatus(int id);
    Task<IEnumerable<AssetsEvent>> GetAssetByStatus(string status);
}
