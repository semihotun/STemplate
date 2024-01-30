using FluentValidation;
using STemplate.Application.Assemblies;
using STemplate.Application.Extension;
using STemplate.Insfrastructure.Utilities.ApiDoc.Swagger;
using STemplate.Insfrastructure.Utilities.Caching.Redis;
using STemplate.Insfrastructure.Utilities.Cors;
using STemplate.Insfrastructure.Utilities.Identity;
using STemplate.Insfrastructure.Utilities.Logging;
using STemplate.Insfrastructure.Utilities.MediatR;
using STemplate.Insfrastructure.Utilities.ServiceBus;
using STemplate.Persistence.Context;
using STemplate.Persistence.Extensions;
using STemplate.Persistence.GenericRepository;
namespace STemplate.Extensions
{
    public static class StartUpInstallExtension
    {
        public static void AddStartupServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.AddCustomSwaggerGen();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.AddCors();
            builder.Services.AddMvc();
            //Log-Cache-Mediatr-FluentValidation-Mass transit
            builder.AddSerilog();
            builder.AddRedis();
            var assembly = ApiAssemblyExtensions.GetLibrariesAssemblies();
            builder.Services.ConfigureDbContext(builder.Configuration);
            builder.AddMediatR(assembly);
            builder.Services.AddValidatorsFromAssembly(ApplicationAssemblyExtension.GetApplicationAssembly(), includeInternalTypes: true);
            builder.AddCustomMassTransit(assembly, (busRegistrationContext, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.AddConsumers(busRegistrationContext);
                busFactoryConfigurator.AddPublishers();
            });
            builder.AddIdentitySettings();
            //Service Registered
            builder.Services.AddScoped<ICoreDbContext, CoreDbContext>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
