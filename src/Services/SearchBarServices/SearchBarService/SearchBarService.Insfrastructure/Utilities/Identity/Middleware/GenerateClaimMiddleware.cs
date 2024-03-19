using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SearchBarService.Insfrastructure.Utilities.AdminRole;

namespace SearchBarService.Insfrastructure.Utilities.Identity.Middleware
{
    public static class OperationClaimCreatorMiddleware
    {
        public static async Task GenerateDbRole(this WebApplication app)
        {
            await Task.Run(async () =>
            {
                var regionName = app.Configuration["RegionName"]?.ToLower();
                var endpoints = app.Services
                    .GetServices<EndpointDataSource>()
                    .SelectMany(x => x.Endpoints)
                        .OfType<RouteEndpoint>()
                    .Where(routeEndpoint => !routeEndpoint.Metadata.OfType<AllowAnonymousAttribute>().Any())
                    .Select(x => x.RoutePattern.RawText?.Replace("api", regionName))
                    .ToArray();
                var _bus = app.Services.GetRequiredService<IBus>();
                if (endpoints.Length != 0)
                {
                    await _bus.Publish<AddAdminRoleIntegrationEvent>(new AddAdminRoleIntegrationEvent(endpoints!));
                }
            });
        }
    }
}
