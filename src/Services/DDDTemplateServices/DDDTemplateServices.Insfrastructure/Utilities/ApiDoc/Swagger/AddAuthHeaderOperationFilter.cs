using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace DDDTemplateServices.Insfrastructure.Utilities.ApiDoc.Swagger
{
    /// <summary>
    /// Add Auth Header 
    /// </summary>
#nullable disable
    public class AddAuthHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation,
            OperationFilterContext context)
        {
            var isAuthorized = context.MethodInfo
                .GetCustomAttributes(true).Any(x => x.GetType().Name == "AuthorizeControl") ||
                context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x.GetType().Name == "AuthorizeControl");
            if (!isAuthorized)
            {
                return;
            }
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            var jwtbearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
            };
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement { [jwtbearerScheme] = new string[] { } }
            };
        }
    }
}
