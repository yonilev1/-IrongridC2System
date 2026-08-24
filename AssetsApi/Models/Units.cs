namespace AssetsApi.Models;

public class Units
{
    public int Id { get; set; }
    public string UnitName { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public List<Assets> Assets { get; set; } = new List<Assets>();
}
