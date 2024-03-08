using AdminIdentityService.Application.Assemblies;
using AdminIdentityService.Application.Extension;
using AdminIdentityService.Domain.Assemblies;
using AdminIdentityService.Insfrastructure.Utilities.ApiDoc.Swagger;
using AdminIdentityService.Insfrastructure.Utilities.Caching.Redis;
using AdminIdentityService.Insfrastructure.Utilities.Cors;
using AdminIdentityService.Insfrastructure.Utilities.Hangfire;
using AdminIdentityService.Insfrastructure.Utilities.Identity;
using AdminIdentityService.Insfrastructure.Utilities.Logging;
using AdminIdentityService.Insfrastructure.Utilities.MediatR;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using AdminIdentityService.Insfrastructure.Utilities.ServiceBus;
using AdminIdentityService.Insfrastructure.Utilities.Telemetry;
using AdminIdentityService.Persistence.Context;
using AdminIdentityService.Persistence.Extensions;
using AdminIdentityService.Persistence.GenericRepository;
using AdminIdentityService.Persistence.SearchEngine;
using AdminIdentityService.Persistence.UnitOfWork;
using Carter;
using FluentValidation;

namespace AdminIdentityService.Extensions;

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
        builder.AddRedis();
        builder.AddTelemeter();
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
        builder.Services.AddTransient<ITokenService, TokenService>();
        builder.Services.AddScoped<ICoreDbContext, CoreDbContext>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        await SearchEngineRegistration.MigrateElasticDbAsync(assembly,builder.Configuration);
        builder.Services.AddScoped<ICoreSearchEngineContext, CoreSearchEngineContext>();
    }
}
