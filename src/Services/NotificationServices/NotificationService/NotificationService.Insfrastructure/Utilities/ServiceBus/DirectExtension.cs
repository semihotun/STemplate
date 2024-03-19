using MassTransit;
using RabbitMQ.Client;

namespace NotificationService.Insfrastructure.Utilities.ServiceBus
{
    /// <summary>
    /// Consumer extension
    /// </summary>
    public static class DirectExtension
    {
        public static void AddDirectConsumer<T>(this IRabbitMqBusFactoryConfigurator cfg,
            Action<IRabbitMqReceiveEndpointConfigurator> addConsumer
            )
            where T : IMessage
        {
            var name = typeof(T).Name.Underscore();
            cfg.ReceiveEndpoint(name,
            re =>
            {
                re.ConfigureConsumeTopology = false;
                // Data Safety
                re.SetQuorumQueue();
                re.Bind(
                    $"{name}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = name;
                        e.ExchangeType = ExchangeType.Direct;
                    }
                );
                addConsumer?.Invoke(re);
                re.RethrowFaultedMessages();
            });
        }
        public static IRabbitMqBusFactoryConfigurator AddDirectProducer<T>(this IRabbitMqBusFactoryConfigurator cfg)
            where T : class, IMessage
        {
            cfg.Message<T>(
              e => e.SetEntityName($"{typeof(T).Name.Underscore()}.input_exchange")
            );
            cfg.Publish<T>(e =>
            {
                e.ExchangeType = ExchangeType.Direct;
                e.Exclude = true;
            });
            cfg.Send<T>(e => e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore()));
            return cfg;
        }
    }
}
