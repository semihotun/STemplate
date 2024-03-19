using BasketService.Application.Assemblies;
using BasketService.Application.Extension;
using BasketService.Insfrastructure.Utilities.ApiDoc.Swagger;
using BasketService.Insfrastructure.Utilities.Caching.Redis;
using BasketService.Insfrastructure.Utilities.Cors;
using BasketService.Insfrastructure.Utilities.HangFire;
using BasketService.Insfrastructure.Utilities.Identity;
using BasketService.Insfrastructure.Utilities.Logging;
using BasketService.Insfrastructure.Utilities.MediatR;
using BasketService.Insfrastructure.Utilities.ServiceBus;
using BasketService.Insfrastructure.Utilities.Telemetry;
using BasketService.Persistence.Context;
using BasketService.Persistence.Extensions;
using BasketService.Persistence.GenericRepository;
using BasketService.Persistence.SearchEngine;
using BasketService.Persistence.UnitOfWork;
using Carter;
using FluentValidation;

namespace BasketService.Extensions
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
