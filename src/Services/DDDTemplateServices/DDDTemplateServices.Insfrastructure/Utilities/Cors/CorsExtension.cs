using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace DDDTemplateServices.Insfrastructure.Utilities.Cors
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
              .WithOrigins("https://localhost:5001",
              "https://localhost:5000",
              "http://localhost:5001",
              "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()));
            return builder;
        }
    }
}
