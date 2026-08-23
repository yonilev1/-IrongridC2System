using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Producer.Models;

namespace Producer.Serviecs;

public class LoadData
{
    public List<LiveAssets>? Load(string path)
    {
        try
        {
            var content = File.ReadAllText(path);
            List<LiveAssets>? assets = JsonSerializer.Deserialize<List<LiveAssets>>(content);
            return assets;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File Not Found");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while reading file: {ex.Message}");
        }
        return null;
    }
}
