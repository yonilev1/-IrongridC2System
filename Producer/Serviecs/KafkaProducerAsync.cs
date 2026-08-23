using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Producer.Models;

namespace Producer.Serviecs;

public class KafkaProducerAsync
{
    private readonly string _bootstrapServer;
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerAsync(string bootstrapServer)
    {
        _bootstrapServer = bootstrapServer;

        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServer
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task EnsureTopicExists(string topic)
    {
        var config = new AdminClientConfig
        {
            BootstrapServers = _bootstrapServer,
        };

        var admin = new AdminClientBuilder(config).Build();

        try
        {
            await admin.CreateTopicsAsync(new[]
            {
                new TopicSpecification
                {
                    Name = topic,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                }
            });
        }
        catch (CreateTopicsException ex)
        {
            if (ex.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
            {
                Console.WriteLine($"Topic {topic} already exists.");
            }
            else
                throw new Exception(ex.Results[0].Error.Reason);
        }
    }

    public async Task<DeliveryResult<Null, string>> SendAsync(string topicName, LiveAssets asset)
    {
        var message = new Message<Null, string>
        {
            Value = asset.ToString()
        };

        var result = await _producer.ProduceAsync(topicName, message);
        Console.WriteLine($"Message sent successfully: Topic: {topicName}, partition: {result.Partition}, offset: {result.Offset}");
        return result;
    }

    public void Flush(TimeSpan time)
    {
        _producer.Flush(time);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}
