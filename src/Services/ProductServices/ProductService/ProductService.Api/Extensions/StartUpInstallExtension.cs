using Carter;
using FluentValidation;
using ProductService.Application.Assemblies;
using ProductService.Application.Extension;
using ProductService.Insfrastructure.Utilities.ApiDoc.Swagger;
using ProductService.Insfrastructure.Utilities.Caching.Redis;
using ProductService.Insfrastructure.Utilities.Cors;
using ProductService.Insfrastructure.Utilities.HangFire;
using ProductService.Insfrastructure.Utilities.Identity;
using ProductService.Insfrastructure.Utilities.Logging;
using ProductService.Insfrastructure.Utilities.MediatR;
using ProductService.Insfrastructure.Utilities.ServiceBus;
using ProductService.Insfrastructure.Utilities.Telemetry;
using ProductService.Persistence.Context;
using ProductService.Persistence.Extensions;
using ProductService.Persistence.GenericRepository;
using ProductService.Persistence.SearchEngine;
using ProductService.Persistence.UnitOfWork;

namespace ProductService.Extensions
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
