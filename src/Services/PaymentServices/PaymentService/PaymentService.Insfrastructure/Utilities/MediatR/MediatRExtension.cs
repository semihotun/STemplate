using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Insfrastructure.Utilities.MediatorBehaviour.Validation;
using System.Reflection;

namespace PaymentService.Insfrastructure.Utilities.MediatR
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
            builderAction?.Invoke(builder);
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            return builder;
        }
    }
}
