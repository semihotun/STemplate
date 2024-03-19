using Microsoft.Extensions.Configuration;
using OrderService.Insfrastructure.Utilities.Kafka;
using OrderService.Insfrastructure.Utilities.Outboxes;

namespace OrderService.Persistence.Cdc.MssqlContext
{
    public static class MssqlDbContextConnectorExtension
    {
        public static async Task AddAllConnectorAsync(IConfiguration configuration)
        {
            await KafkaExtension.AddConnector(configuration,
                SkippedOperation.Delete,
                "Outbox",
                nameof(Outbox));
        }
    }
}
