using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CustomerService.Insfrastructure.Utilities.ApiDoc.Swagger
{
    /// <summary>
    /// Add Swagger
    /// </summary>
    public static class SwaggerServiceCollectionExtension
    {
        public static void AddCustomSwaggerGen(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = builder.Configuration["RegionName"],
                    Description = builder.Configuration["RegionName"] + "Web API Project",
                });
                c.OperationFilter<AddAuthHeaderOperationFilter>();
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = "Bearer Header",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });
            });
        }
    }
}
