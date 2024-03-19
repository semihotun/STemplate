using Carter;
using FluentValidation;
using PaymentService.Application.Assemblies;
using PaymentService.Application.Extension;
using PaymentService.Insfrastructure.Utilities.ApiDoc.Swagger;
using PaymentService.Insfrastructure.Utilities.Caching.Redis;
using PaymentService.Insfrastructure.Utilities.Cors;
using PaymentService.Insfrastructure.Utilities.HangFire;
using PaymentService.Insfrastructure.Utilities.Identity;
using PaymentService.Insfrastructure.Utilities.Logging;
using PaymentService.Insfrastructure.Utilities.MediatR;
using PaymentService.Insfrastructure.Utilities.ServiceBus;
using PaymentService.Insfrastructure.Utilities.Telemetry;
using PaymentService.Persistence.Context;
using PaymentService.Persistence.Extensions;
using PaymentService.Persistence.GenericRepository;
using PaymentService.Persistence.SearchEngine;
using PaymentService.Persistence.UnitOfWork;

namespace PaymentService.Extensions
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
