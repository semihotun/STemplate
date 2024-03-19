using Microsoft.Extensions.Configuration;
using ProductService.Insfrastructure.Utilities.Kafka;
using ProductService.Insfrastructure.Utilities.Outboxes;

namespace ProductService.Persistence.Cdc.MssqlContext
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
