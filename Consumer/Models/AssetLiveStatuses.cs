using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Models;

public class AssetLiveStatuses
{
    public int AssetId { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public string RawValue { get; set; } = string.Empty;
    public string ProcessedStatus { get; set; } = string.Empty;
    public bool IsVerified { get; set; } 
    public DateTime LastUpdate { get; set; }
}
