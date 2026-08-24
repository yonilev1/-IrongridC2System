namespace AssetsApi.Dtos;

public class UpdateAsset
{
    public int UnitId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
}
