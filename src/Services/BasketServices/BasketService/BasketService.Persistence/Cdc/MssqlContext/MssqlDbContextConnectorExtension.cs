using BasketService.Insfrastructure.Utilities.Kafka;
using BasketService.Insfrastructure.Utilities.Outboxes;
using Microsoft.Extensions.Configuration;

namespace BasketService.Persistence.Cdc.MssqlContext
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
