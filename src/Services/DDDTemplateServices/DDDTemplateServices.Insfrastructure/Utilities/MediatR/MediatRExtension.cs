using DDDTemplateServices.Insfrastructure.Utilities.MediatorBehaviour.Performance;
using DDDTemplateServices.Insfrastructure.Utilities.MediatorBehaviour.Validation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace DDDTemplateServices.Insfrastructure.Utilities.MediatR
{
    /// <summary>
    /// mediatr add to webbapp
    /// </summary>
    public static class MediatRExtension
    {
        public static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder,
            Assembly[] assembly, Action<WebApplicationBuilder>? builderAction = null)
        {
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            builderAction?.Invoke(builder);
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            return builder;
        }
    }
}
