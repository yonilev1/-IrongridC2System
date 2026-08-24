using AssetsApi.Models;
using System.Text.Json.Serialization;

namespace AssetsApi.Dtos;

public class AssetWithStatus
{
    public int Id { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public string UnitName { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string PtocessedStatus { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTime LastUpdate { get; set; }
}
