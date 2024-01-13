using AdminIdentityService.Application.IntegrationEvents.AdminRoles;
using AdminIdentityService.Insfrastructure.Utilities.AdminRole;
using AdminIdentityService.Insfrastructure.Utilities.ServiceBus;
using MassTransit;
namespace AdminIdentityService.Application.Extension
{
    /// <summary>
    /// Consumer Or Producer Add
    /// </summary>
    public static class AddConsumerOrProducer
    {
        public static void AddPublishers(this IRabbitMqBusFactoryConfigurator cfg)
        {
        }
        public static void AddConsumers(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {
            cfg.AddDirectConsumer<AddAdminRoleIntegrationEvent>((x) => x.ConfigureConsumer<AddAdminRoleIntegrationEventHandler>(ctx));
        }
    }
}
