using CatalogService.Insfrastructure.Utilities.Kafka;
using CatalogService.Insfrastructure.Utilities.Outboxes;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Persistence.Cdc.MssqlContext
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
