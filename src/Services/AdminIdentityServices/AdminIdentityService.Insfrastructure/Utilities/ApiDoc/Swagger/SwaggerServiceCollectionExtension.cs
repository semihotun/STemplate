using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
namespace AdminIdentityService.Insfrastructure.Utilities.ApiDoc.Swagger
{
    /// <summary>
    /// Add Swagger
    /// </summary>
    public static class SwaggerServiceCollectionExtension
    {
        public static void AddCustomSwaggerGen(this IServiceCollection services,
            string Title,
            string Description)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = Title,
                    Description = Description,
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
