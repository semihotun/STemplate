using Microsoft.Extensions.Configuration;
using NotificationService.Insfrastructure.Utilities.Kafka;
using NotificationService.Insfrastructure.Utilities.Outboxes;

namespace NotificationService.Persistence.Cdc.MssqlContext
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
