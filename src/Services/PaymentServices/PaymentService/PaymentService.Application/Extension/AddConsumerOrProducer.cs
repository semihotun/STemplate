using MassTransit;
using PaymentService.Insfrastructure.Utilities.AdminRole;
using PaymentService.Insfrastructure.Utilities.ServiceBus;

namespace PaymentService.Application.Extension
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
