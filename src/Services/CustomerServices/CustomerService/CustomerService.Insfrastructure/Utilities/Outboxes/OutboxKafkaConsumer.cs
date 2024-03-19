using CustomerService.Insfrastructure.Utilities.Kafka;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;

namespace CustomerService.Insfrastructure.Utilities.Outboxes
{
    public static class OutboxKafkaConsumer
    {
#nullable disable
        /// <summary>
        /// https://www.youtube.com/watch?v=bOOYMAvX_po&t=302s
        /// https://debezium.io/documentation/reference/stable/connectors/sqlserver.html
        /// </summary>
        /// <param name="webApplication"></param>
        /// <param name="applicationAssembly"></param>
        /// <returns></returns>
        public static async Task AddOutboxKafkaConsumerAsync(this WebApplication webApplication, Assembly applicationAssembly)
        {
            using var consumer = KafkaConsumerExtension.CreateKafkaConsumer(webApplication.Configuration);
            consumer.Subscribe($"{webApplication.Configuration["RegionName"]}.{webApplication.Configuration["RegionName"]}.dbo.Outbox");
            var bus = webApplication.Services.GetRequiredService<IBus>();
            var saveState = KafkaOutboxErrorState.NoError;
            while (true)
            {
                var source = new CancellationTokenSource();
                try
                {
                    var consume = consumer.Consume();
                    var result = JsonConvert.DeserializeObject<KafkaOutboxModel>(consume.Message.Value);
                    var type = applicationAssembly.GetType(result.Payload.After.IntegrationEventType);
                    var obj = JsonConvert.DeserializeObject(result.Payload.After.Content, type, new JsonSerializerSettings
                    {
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    });
                    await bus.Publish(obj);
                    saveState = KafkaOutboxErrorState.KafkaError;
                    consumer.Commit(consume);
                    saveState = KafkaOutboxErrorState.NoError;
                }
                catch
                {
                    if (saveState == KafkaOutboxErrorState.KafkaError)
                    {
                        source.Cancel();
                    }
                }
            }
        }
    }
}
