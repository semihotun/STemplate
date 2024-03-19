using Microsoft.Extensions.Configuration;
using ShippingService.Insfrastructure.Utilities.Kafka;
using ShippingService.Insfrastructure.Utilities.Outboxes;

namespace ShippingService.Persistence.Cdc.MssqlContext
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
