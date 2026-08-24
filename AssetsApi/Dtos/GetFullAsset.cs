using AssetsApi.Models;
using System.Text.Json.Serialization;

namespace AssetsApi.Dtos;

public class GetFullAsset
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    [JsonIgnore]
    public AssetLiveStatuses? LiveAssets { get; set; }
}
