using Consumer.Data;
using Consumer.Models;
using System.Text.Json;

namespace Consumer.Service;

public class ProcesserAsync
{
    private readonly AssetsDbContext _context;

    public ProcesserAsync(AssetsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ProcessDataAsync(string assets)
    {
        try
        {
            var asset = JsonSerializer.Deserialize<LiveAssets>(assets);

            if (asset == null)
                return false;

            var older = _context.AssetLiveStatuses.FirstOrDefault(t => t.AssetId == asset.AssetId);
            string rawValue = asset.RawValue;
            string processedStatus = "warning";
            bool isVerified = false;

            if (asset.AssetType == "PerimeterSensor")
            {

                if (asset.RawValue.ToLower() == "bad" || asset.RawValue == "bed")
                {
                    rawValue = "Bad";
                    processedStatus = "Warning";
                    isVerified = true;
                }
                else if (asset.RawValue.ToLower() == "good" || asset.RawValue == "gud")
                {
                    rawValue = "Good";
                    processedStatus = "Stable";
                    isVerified = true;
                }
                else
                {
                    processedStatus = "Warning";
                    isVerified = false;
                }
            }
            else if (asset.AssetType == "UAV")
            {
                int battery;
                if (!int.TryParse(asset.RawValue, out battery) || battery > 100 || battery < 0)
                {
                    processedStatus = "Warning";
                    isVerified = false;
                }
                else if (battery >= 20 && battery <= 100)
                {
                    processedStatus = "Stable";
                    isVerified = true;
                }
                else if (battery <= 19)
                {
                    processedStatus = "Warning";
                    isVerified = true;
                }
            }

            AssetLiveStatuses fullAsset = new AssetLiveStatuses
            {
                AssetId = asset.AssetId,
                AssetType = asset.AssetType,
                RawValue = rawValue,
                ProcessedStatus = processedStatus,
                IsVerified = isVerified,
                LastUpdate = asset.Timestamp
            };

            if (older == null)
            {
                await _context.AssetLiveStatuses.AddAsync(fullAsset);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Saved new Asset from Type: {fullAsset.AssetType} in to Db. Asset Id: {fullAsset.AssetId}, Asset Type: {fullAsset.AssetType}.");
                return true;
            }
            else
            {
                if (older.LastUpdate < fullAsset.LastUpdate)
                {
                    older.IsVerified = fullAsset.IsVerified;
                    older.LastUpdate = fullAsset.LastUpdate;
                    older.ProcessedStatus = fullAsset.ProcessedStatus;
                    older.RawValue = fullAsset.RawValue;
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Updated Asset in Db. Asset Id: {fullAsset.AssetId}, Asset Type: {fullAsset.AssetType}.");
                    return true;
                }
                else
                {
                    Console.WriteLine("There is a newer report from this asset. didnt add this report to Db.");
                }
            }
            return false;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine("argument exception");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"In Service, {ex.InnerException}");
        }
        return false;
    }
}