using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
namespace Web.ApiGateway.Extension
{
    public static class OcelotExtension
    {
        /// <summary>
        /// Add ocelot to Development
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddOcelotDevelopmentSetting(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("Configurations/ocelot.json", optional: false, reloadOnChange: false)
                                 .AddJsonFile("Configurations/ocelot.Development.json", optional: false, reloadOnChange: false);
            builder.Services.AddOcelot(builder.Configuration).AddPolly();
            return builder;
        }
        /// <summary>
        /// Add Ocelot Production
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddOcelotProductionSetting(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("Configurations/ocelot.json", optional: false, reloadOnChange: false)
                                 .AddJsonFile("Configurations/ocelot.Production.json", optional: false, reloadOnChange: false);
            builder.Services.AddOcelot(builder.Configuration).AddPolly();
            return builder;
        }
    }
}
