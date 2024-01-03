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
              .WithOrigins("https://localhost:4000",
              "https://localhost:4033",
              "http://localhost:4000",
              "http://localhost:4033")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()));
            return builder;
        }
    }
}
