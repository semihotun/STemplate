using Carter;
using FluentValidation;
using OrderService.Application.Assemblies;
using OrderService.Application.Extension;
using OrderService.Insfrastructure.Utilities.ApiDoc.Swagger;
using OrderService.Insfrastructure.Utilities.Caching.Redis;
using OrderService.Insfrastructure.Utilities.Cors;
using OrderService.Insfrastructure.Utilities.HangFire;
using OrderService.Insfrastructure.Utilities.Identity;
using OrderService.Insfrastructure.Utilities.Logging;
using OrderService.Insfrastructure.Utilities.MediatR;
using OrderService.Insfrastructure.Utilities.ServiceBus;
using OrderService.Insfrastructure.Utilities.Telemetry;
using OrderService.Persistence.Context;
using OrderService.Persistence.Extensions;
using OrderService.Persistence.GenericRepository;
using OrderService.Persistence.SearchEngine;
using OrderService.Persistence.UnitOfWork;

namespace OrderService.Extensions
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
