using Microsoft.Extensions.Configuration;
using SearchBarService.Insfrastructure.Utilities.Kafka;
using SearchBarService.Insfrastructure.Utilities.Outboxes;

namespace SearchBarService.Persistence.Cdc.MssqlContext
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
