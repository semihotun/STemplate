using Carter;
using FluentValidation;
using SearchBarService.Application.Assemblies;
using SearchBarService.Application.Extension;
using SearchBarService.Insfrastructure.Utilities.ApiDoc.Swagger;
using SearchBarService.Insfrastructure.Utilities.Caching.Redis;
using SearchBarService.Insfrastructure.Utilities.Cors;
using SearchBarService.Insfrastructure.Utilities.HangFire;
using SearchBarService.Insfrastructure.Utilities.Identity;
using SearchBarService.Insfrastructure.Utilities.Logging;
using SearchBarService.Insfrastructure.Utilities.MediatR;
using SearchBarService.Insfrastructure.Utilities.ServiceBus;
using SearchBarService.Insfrastructure.Utilities.Telemetry;
using SearchBarService.Persistence.Context;
using SearchBarService.Persistence.Extensions;
using SearchBarService.Persistence.GenericRepository;
using SearchBarService.Persistence.SearchEngine;
using SearchBarService.Persistence.UnitOfWork;

namespace SearchBarService.Extensions
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
