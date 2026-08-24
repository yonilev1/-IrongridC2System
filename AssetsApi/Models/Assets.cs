namespace AssetsApi.Models;

public class Assets
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public Units Unit { get; set; }
    public List<AssetLiveStatuses> LiveAssets { get; set } = new List<AssetLiveStatuses>();
}
