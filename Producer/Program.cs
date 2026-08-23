using Microsoft.Extensions.Configuration;
using Producer.Models;
using Producer.Serviecs;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {

            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Build();

            string bootstrapServer = configuration["Kafka:BootstrapServer"] ?? "localhost:9092";
            string UavTopic = configuration["Kafka:Topics:Uav"] ?? "uav-event";
            string SensorTopic = configuration["Kafka:Topics:Sensor"] ?? "sensor-event";

            LoadData loader = new LoadData();
            List<LiveAssets>? assets = loader.Load(@"C:\Users\Yonil\IrongridC2System\Producer\Data\field_reports.json");

            if (assets == null)
                throw new Exception("Error while opening file");

            KafkaProducerAsync producer = new KafkaProducerAsync(bootstrapServer);

            await producer.EnsureTopicExists(UavTopic);
            await producer.EnsureTopicExists(SensorTopic);
            foreach (LiveAssets asset in assets)
            {
                if (asset.AssetType == "UAV")
                {
                    var result = await producer.SendAsync(UavTopic, asset);
                }
                else if (asset.AssetType == "PerimeterSensor")
                {
                    var result = await producer.SendAsync(SensorTopic, asset);
                }
            } 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error Acured: {ex.Message}");
        }
    }
}