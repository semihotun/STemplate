using Microsoft.Extensions.Configuration;
using PaymentService.Insfrastructure.Utilities.Kafka;
using PaymentService.Insfrastructure.Utilities.Outboxes;

namespace PaymentService.Persistence.Cdc.MssqlContext
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
