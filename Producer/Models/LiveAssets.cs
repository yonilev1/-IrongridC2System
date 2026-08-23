using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Models;

public class LiveAssets
{
    public int Id { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public string RawValue { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
}
