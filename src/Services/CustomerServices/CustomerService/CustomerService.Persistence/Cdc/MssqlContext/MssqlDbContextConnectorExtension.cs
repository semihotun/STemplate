using CustomerService.Insfrastructure.Utilities.Kafka;
using CustomerService.Insfrastructure.Utilities.Outboxes;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Persistence.Cdc.MssqlContext
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
