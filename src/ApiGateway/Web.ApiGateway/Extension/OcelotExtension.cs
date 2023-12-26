using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
namespace Web.ApiGateway.Extension
{
    public static class OcelotExtension
    {
        /// <summary>
        /// Add ocelot to webapp
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddOcelotSetting(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile($"Configurations/ocelot.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration).AddPolly();
            return builder;
        }
    }
}
