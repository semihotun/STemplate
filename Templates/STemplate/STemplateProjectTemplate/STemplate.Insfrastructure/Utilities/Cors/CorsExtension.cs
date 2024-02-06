using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace STemplate.Insfrastructure.Utilities.Cors
{
    /// <summary>
    /// appgateway and service port added to cors
    /// </summary>
    public static class CorsExtension
    {
        public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
              options.AddDefaultPolicy(policy => policy
                .WithOrigins($"https://{builder.Configuration["HostAdress"]}:4000",
                $"http://{builder.Configuration["HostAdress"]}:4000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));
                return builder;
        }
    }
}
