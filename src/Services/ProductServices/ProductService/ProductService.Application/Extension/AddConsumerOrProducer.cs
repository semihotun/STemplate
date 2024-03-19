using MassTransit;
using ProductService.Insfrastructure.Utilities.AdminRole;
using ProductService.Insfrastructure.Utilities.ServiceBus;

namespace ProductService.Application.Extension
{
    /// <summary>
    ///  Add Consumer Or Producer
    /// </summary>
    public static class AddConsumerOrProducer
    {
        public static void AddPublishers(this IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.AddDirectProducer<AddAdminRoleIntegrationEvent>();
        }
        public static void AddConsumers(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {
        }
    }
}
