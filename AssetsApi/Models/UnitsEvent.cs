namespace AssetsApi.Models;

public class UnitsEvent
{
    public int Id { get; set; }
    public string UnitName { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public List<AssetsEvent> Assets { get; set; } = new List<AssetsEvent>();
}
