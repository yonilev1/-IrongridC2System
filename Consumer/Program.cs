using Confluent.Kafka;
using Consumer.Data;
using Consumer.Models;
using Consumer.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

        var service = new ServiceCollection();

        service.AddDbContext<AssetsDbContext>(options =>
        options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

        service.AddScoped<ProcesserAsync>();

        var serviceProvider = service.BuildServiceProvider();

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServer"],
            GroupId = configuration["Kafka:GroupId"],
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        }; 
    }
}