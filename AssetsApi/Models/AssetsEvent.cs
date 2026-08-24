using System.Text.Json.Serialization;

namespace AssetsApi.Models;

public class AssetsEvent
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    [JsonIgnore]
    public UnitsEvent Unit { get; set; } = null!;
    //[JsonIgnore]
    public AssetLiveStatuses? LiveAssets { get; set; }
}
