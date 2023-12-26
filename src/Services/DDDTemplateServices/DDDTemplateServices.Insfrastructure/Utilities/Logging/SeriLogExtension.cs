using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
namespace DDDTemplateServices.Insfrastructure.Utilities.Logging;
public static class SerilogExtension
{
    /// <summary>
    /// https://medium.com/@ademolguner/net-coreda-serilog-i%CC%87le-elasticsearch-sink-%C3%B6zelle%C5%9Ftirmesi-2-368a49957a6e
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", $"eCommerce")
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails()
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .WriteTo.Async(writeTo => writeTo.Console(new JsonFormatter()))
            .WriteTo.Async(writeTo => writeTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"] ?? "http://host.docker.internal:9200"))
                {
                    TypeName = null,
                    AutoRegisterTemplate = true,
                    IndexFormat = $"eCommerceLogs-{DateTime.UtcNow:yyyy-MM}",
                    BatchAction = ElasticOpType.Create,
                    CustomFormatter = new ElasticsearchJsonFormatter(),
                    OverwriteTemplate = true,
                    DetectElasticsearchVersion = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    NumberOfReplicas = 1,
                    NumberOfShards = 2,
                    FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                                           EmitEventFailureHandling.WriteToFailureSink |
                                                           EmitEventFailureHandling.RaiseCallback |
                                                           EmitEventFailureHandling.ThrowException
                }))
           .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger, true);
        return builder;
    }
}