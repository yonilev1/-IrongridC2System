using Confluent.Kafka;
using Consumer.Data;
using Consumer.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("=== starting to read from kafka ===");
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"C:\Users\Yonil\IrongridC2System\Consumer\appsettings.json", optional: false)
            .Build();

            var service = new ServiceCollection();

            service.AddDbContext<AssetsDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

            service.AddScoped<ProcesserAsync>();

            var serviceProvider = service.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AssetsDbContext>();
                db.Database.EnsureCreated();
            }

            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServer"],
                GroupId = configuration["Kafka:GroupId"],
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Null, string>(config).Build();

            

            while (true)
            {
                try
                {
                    consumer.Subscribe(configuration["Kafka:Topics:Uav"]);
                    Console.WriteLine("Reading From Kafka");
                    var readingUav = consumer.Consume(TimeSpan.FromSeconds(1));
                    if (readingUav == null || readingUav.Message?.Value == null)
                        continue;

                    Console.WriteLine($"Got reading: {readingUav.Message.Value}");
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var process = scope.ServiceProvider.GetRequiredService<ProcesserAsync>();

                        var success = await process.ProcessDataAsync(readingUav.Message.Value);
                        consumer.Commit();
                    }
                    consumer.Unsubscribe();

                    consumer.Subscribe(configuration["Kafka:Topics:Sensor"]);
                    Console.WriteLine("Getting Data");
                    var readingSensor = consumer.Consume(TimeSpan.FromSeconds(1));

                    if (readingSensor == null || readingSensor.Message?.Value == null)
                        continue;

                    Console.WriteLine($"Got reading: {readingSensor.Message.Value}");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var process = scope.ServiceProvider.GetRequiredService<ProcesserAsync>();

                        var success = await process.ProcessDataAsync(readingSensor.Message.Value);
                        consumer.Commit();
                    }
                    consumer.Unsubscribe();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"In Loop in program {ex.Message}");
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}