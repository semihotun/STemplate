using DDDTemplateService.Application.Assemblies;
using DDDTemplateService.Application.Extension;
using DDDTemplateServices.Insfrastructure.Utilities.Caching.Redis;
using DDDTemplateServices.Insfrastructure.Utilities.Cors;
using DDDTemplateServices.Insfrastructure.Utilities.Identity;
using DDDTemplateServices.Insfrastructure.Utilities.Logging;
using DDDTemplateServices.Insfrastructure.Utilities.MediatR;
using DDDTemplateServices.Insfrastructure.Utilities.ServiceBus;
using DDDTemplateServices.Persistence.Context;
using DDDTemplateServices.Persistence.Extensions;
using DDDTemplateServices.Persistence.GenericRepository;
using FluentValidation;
namespace DDDTemplateService.Extensions
{
    public static class StartUpInstallExtension
    {
        public static void AddStartupServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
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
