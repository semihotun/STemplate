using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace NotificationService.Insfrastructure.Utilities.Kafka
{
    public static class KafkaConsumerExtension
    {
        public static IConsumer<Ignore, string> CreateKafkaConsumer(IConfiguration configuration)
        {
            return new ConsumerBuilder<Ignore, string>(new ConsumerConfig()
            {
                GroupId = configuration["RegionName"],
                BootstrapServers = $"s_kafka:{configuration["Kafka_External_Tcp"]}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            }).Build();
        }
    }
}
