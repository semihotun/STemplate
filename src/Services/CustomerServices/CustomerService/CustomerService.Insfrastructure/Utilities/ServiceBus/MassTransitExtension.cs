using MassTransit;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace CustomerService.Insfrastructure.Utilities.ServiceBus
{
    public static class MassTransitExtension
    {
        /// <summary>
        /// MassTransit Setting
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        /// <param name="configureReceiveEndpoints"></param>
        /// <param name="configureBusRegistration"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddCustomMassTransit(
        this WebApplicationBuilder builder,
        Assembly[] assembly,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureReceiveEndpoints = null,
        Action<IBusRegistrationConfigurator>? configureBusRegistration = null)
        {
            builder.Services.AddMassTransit(ConfiguratorAction);
            void ConfiguratorAction(IBusRegistrationConfigurator busRegistrationConfigurator)
            {
                configureBusRegistration?.Invoke(busRegistrationConfigurator);
                busRegistrationConfigurator.AddConsumers(assembly);
                busRegistrationConfigurator.SetEndpointNameFormatter(new SnakeCaseEndpointNameFormatter(false));
                var configuration = builder.Configuration;
                busRegistrationConfigurator.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host($"{configuration["RabbitMQ:HostName"]}:{configuration["RabbitMQ:Port"]}", opt =>
                    {
                        opt.Username(configuration["RabbitMQ:UserName"]);
                        opt.Password(configuration["RabbitMQ:Password"]);
                    });
                    cfg.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter());
                    configureReceiveEndpoints?.Invoke(ctx, cfg);
                });
            }
            return builder;
        }
    }
}
