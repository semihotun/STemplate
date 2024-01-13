using DDDTemplateServices.Insfrastructure.Utilities.AdminRole;
using MassTransit;
using MassTransit.Internals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace DDDTemplateServices.Insfrastructure.Utilities.Identity.Middleware
{
    public static class OperationClaimCreatorMiddleware
    {
        public static async Task GenerateDbRole(this WebApplication app, Assembly assembly)
        {
            var _bus = app.Services.GetRequiredService<IBus>();
            var regionName = app.Configuration["RegionName"]?.ToLower();
            var methods = assembly.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(x => (x.ReturnType.GetTypeName() == "System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>" ||
                             x.ReturnType.GetTypeName() == "Microsoft.AspNetCore.Mvc.IActionResult") &&
                             x.GetCustomAttribute<AllowAnonymousAttribute>() == null)
                .Select(data =>
                {
                    return Task.Run(() =>
                    {
                        var controllerName = data.DeclaringType?.Name.Replace("Controller", "").ToLower();
                        var httpMethodAttribute = data.GetCustomAttributes().OfType<HttpMethodAttribute>().FirstOrDefault();
                        var methodPath = httpMethodAttribute != null
                          ? $"{regionName}/{controllerName}/{httpMethodAttribute.Template}"
                          : $"{regionName}/{controllerName}/{data.Name}";
                        return methodPath;
                    });
                });
            var integrationEvent = new AddAdminRoleIntegrationEvent(await Task.WhenAll(methods));
            if (integrationEvent.RoleName.Length != 0)
            {
                await _bus.Publish<AddAdminRoleIntegrationEvent>(integrationEvent);
            }
        }
    }
}
