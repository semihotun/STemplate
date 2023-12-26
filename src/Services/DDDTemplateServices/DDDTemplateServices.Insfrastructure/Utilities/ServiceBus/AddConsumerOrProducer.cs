using MassTransit;
namespace DDDTemplateServices.Insfrastructure.Utilities.ServiceBus
{
    /// <summary>
    /// 
    /// </summary>
    public static class AddConsumerOrProducer
    {
        public static void AddPublishers(this IRabbitMqBusFactoryConfigurator cfg)
        {
            //cfg.AddDirectProducer<SelectionItem>();
        }
        public static void AddConsumers(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {
            //cfg.AddDirectConsumer<SelectionItem>((x) => x.ConfigureConsumer<GetMailIntegrationEventHandler>(ctx));
        }
    }
}
