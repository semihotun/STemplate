using MassTransit;
using NotificationService.Insfrastructure.Utilities.AdminRole;
using NotificationService.Insfrastructure.Utilities.ServiceBus;

namespace NotificationService.Application.Extension
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
