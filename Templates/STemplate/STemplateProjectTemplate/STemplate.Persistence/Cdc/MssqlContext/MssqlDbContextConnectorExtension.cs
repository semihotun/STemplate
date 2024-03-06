using Microsoft.Extensions.Configuration;
using STemplate.Insfrastructure.Utilities.Kafka;
using STemplate.Insfrastructure.Utilities.Outboxes;

namespace STemplate.Persistence.Cdc.MssqlContext;

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
