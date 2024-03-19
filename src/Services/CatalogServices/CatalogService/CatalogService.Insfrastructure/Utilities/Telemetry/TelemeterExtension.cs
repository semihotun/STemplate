using MassTransit.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CatalogService.Insfrastructure.Utilities.Telemetry
{
    public static class TelemeterExtension
    {
        public static void AddTelemeter(this WebApplicationBuilder builder)
        {
            var regionName = builder.Configuration["RegionName"];
            void configureResource(ResourceBuilder r) => r.AddService(
            serviceName: regionName!,
            serviceVersion: "1.0",
            serviceInstanceId: Environment.MachineName);

            builder.Services.AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(b => b
                .AddSource(DiagnosticHeaders.DefaultListenerName) // MassTransit ActivitySource
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                //.AddConsoleExporter()
                .AddZipkinExporter(regionName, (opt) =>
                {
                    opt.Endpoint = new Uri("http://s_zipkin:9411/api/v2/spans");
                })
            );
        }
    }
}
