using Carter;
using FluentValidation;
using NotificationService.Application.Assemblies;
using NotificationService.Application.Extension;
using NotificationService.Insfrastructure.Utilities.ApiDoc.Swagger;
using NotificationService.Insfrastructure.Utilities.Caching.Redis;
using NotificationService.Insfrastructure.Utilities.Cors;
using NotificationService.Insfrastructure.Utilities.HangFire;
using NotificationService.Insfrastructure.Utilities.Identity;
using NotificationService.Insfrastructure.Utilities.Logging;
using NotificationService.Insfrastructure.Utilities.MediatR;
using NotificationService.Insfrastructure.Utilities.ServiceBus;
using NotificationService.Insfrastructure.Utilities.Telemetry;
using NotificationService.Persistence.Context;
using NotificationService.Persistence.Extensions;
using NotificationService.Persistence.GenericRepository;
using NotificationService.Persistence.SearchEngine;
using NotificationService.Persistence.UnitOfWork;

namespace NotificationService.Extensions
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
