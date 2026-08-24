namespace AssetsApi.Models;

public class AssetsEvent
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public UnitsEvent Unit { get; set; } = null!;
    public List<AssetLiveStatuses> LiveAssets { get; set; } = new List<AssetLiveStatuses>();
}
