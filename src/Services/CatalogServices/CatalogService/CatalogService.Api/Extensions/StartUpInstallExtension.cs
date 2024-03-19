using Carter;
using CatalogService.Application.Assemblies;
using CatalogService.Application.Extension;
using CatalogService.Insfrastructure.Utilities.ApiDoc.Swagger;
using CatalogService.Insfrastructure.Utilities.Caching.Redis;
using CatalogService.Insfrastructure.Utilities.Cors;
using CatalogService.Insfrastructure.Utilities.HangFire;
using CatalogService.Insfrastructure.Utilities.Identity;
using CatalogService.Insfrastructure.Utilities.Logging;
using CatalogService.Insfrastructure.Utilities.MediatR;
using CatalogService.Insfrastructure.Utilities.ServiceBus;
using CatalogService.Insfrastructure.Utilities.Telemetry;
using CatalogService.Persistence.Context;
using CatalogService.Persistence.Extensions;
using CatalogService.Persistence.GenericRepository;
using CatalogService.Persistence.SearchEngine;
using CatalogService.Persistence.UnitOfWork;
using FluentValidation;

namespace CatalogService.Extensions
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
