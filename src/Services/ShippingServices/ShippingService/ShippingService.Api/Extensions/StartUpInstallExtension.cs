using Carter;
using FluentValidation;
using ShippingService.Application.Assemblies;
using ShippingService.Application.Extension;
using ShippingService.Insfrastructure.Utilities.ApiDoc.Swagger;
using ShippingService.Insfrastructure.Utilities.Caching.Redis;
using ShippingService.Insfrastructure.Utilities.Cors;
using ShippingService.Insfrastructure.Utilities.HangFire;
using ShippingService.Insfrastructure.Utilities.Identity;
using ShippingService.Insfrastructure.Utilities.Logging;
using ShippingService.Insfrastructure.Utilities.MediatR;
using ShippingService.Insfrastructure.Utilities.ServiceBus;
using ShippingService.Insfrastructure.Utilities.Telemetry;
using ShippingService.Persistence.Context;
using ShippingService.Persistence.Extensions;
using ShippingService.Persistence.GenericRepository;
using ShippingService.Persistence.SearchEngine;
using ShippingService.Persistence.UnitOfWork;

namespace ShippingService.Extensions
{
    public static class StartUpInstallExtension
    {
        public static async Task AddStartupServicesAsync(this WebApplicationBuilder builder)
        {
            builder.Services.AddCarter();
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.AddCustomSwaggerGen();
            builder.AddCors();
            builder.Services.AddMvc();
            //Log-Cache-Mediatr-FluentValidation-Mass transit
            builder.AddSerilog();
            builder.AddTelemeter();
            builder.AddRedis();
            var assembly = ApiAssemblyExtensions.GetLibrariesAssemblies();
            builder.AddHangFire(assembly);
            await builder.Services.ConfigureDbContextAsync(builder.Configuration);
            builder.AddMediatR(assembly);
            builder.Services.AddValidatorsFromAssembly(ApplicationAssemblyExtension.GetApplicationAssembly(), includeInternalTypes: true);
            builder.AddCustomMassTransit(assembly, (busRegistrationContext, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.AddConsumers(busRegistrationContext);
                busFactoryConfigurator.AddPublishers();
            });
            builder.AddIdentitySettings();
            //Service Registered
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICoreDbContext, CoreDbContext>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            await SearchEngineRegistration.MigrateElasticDbAsync(assembly, builder.Configuration);
            builder.Services.AddScoped<ICoreSearchEngineContext, CoreSearchEngineContext>();
        }
    }
}
