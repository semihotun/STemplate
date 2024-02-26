using Microsoft.Extensions.Configuration;
namespace AdminIdentityService.Insfrastructure.Utilities.Kafka;
public static class KafkaExtension
{
    public static async Task AddConnector(IConfiguration configuration)
    {
        await new HttpClient().PostAsync($"http://host.docker.internal:{configuration["Debeziumconnect_Port"]}/connectors",
        new StringContent($@"
            {{
              ""name"": ""{configuration["RegionName"]}.connector"",
              ""config"": {{
                ""topic.prefix"": ""{configuration["RegionName"]}"",
                ""connector.class"": ""io.debezium.connector.sqlserver.SqlServerConnector"",
                ""schema.history.internal.kafka.topic"": ""{configuration["RegionName"]}.schema"",
                ""database.names"": ""{configuration["RegionName"]}"",
                ""table.include.list"": ""dbo.Outbox"",
                ""database.hostname"": ""s_sqlserver"",
                ""database.port"": ""{configuration["SQL_TCP_Port"]}"",
                ""database.user"": ""{configuration["SQL_User"]}"",
                ""database.password"": ""{configuration["SQL_Password"]}"",
                ""schema.history.internal.kafka.bootstrap.servers"": ""s_kafka:{configuration["Kafka_TCP_Port"]}"",
                ""database.encrypt"": false,
                ""databse.trustServerCertificate"": true,
                ""skipped.operations"": ""d"",
                ""schema.history.internal.kafka.query.timeout.ms"":3000,
                ""snapshot.mode"": ""initial""
              }}
            }}", System.Text.Encoding.UTF8, "application/json"));
    }
}
