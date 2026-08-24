namespace AssetsApi.Dtos;

public class AssetsStatusPerUnit
{
    public int AssetId { get; set; }
    public string AssetSerial { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public string? PtocessedStatus { get; set; } = string.Empty;
    public bool? IsVerified { get; set; }
    public DateTime? LastUpdate { get; set; }
}
