using CustomerService.Insfrastructure.Utilities.AdminRole;
using CustomerService.Insfrastructure.Utilities.ServiceBus;
using MassTransit;

namespace CustomerService.Application.Extension
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
