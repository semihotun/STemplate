using DDDTemplateServices.Insfrastructure.Utilities.AdminRole;
using DDDTemplateServices.Insfrastructure.Utilities.ServiceBus;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDTemplateService.Application.Extension
{
    /// <summary>
    /// 
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
