using AdminIdentityService.Insfrastructure.Utilities.Kafka;
using AdminIdentityService.Insfrastructure.Utilities.Outboxes;
using Microsoft.Extensions.Configuration;

namespace AdminIdentityService.Persistence.Cdc.MssqlContext
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
