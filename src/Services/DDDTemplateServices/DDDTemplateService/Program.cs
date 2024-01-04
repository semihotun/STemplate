using DDDTemplateService.Application.Assemblies;
using DDDTemplateService.Application.Extension;
using DDDTemplateService.Extensions;
using DDDTemplateServices.Insfrastructure.Utilities.Caching.Redis;
using DDDTemplateServices.Insfrastructure.Utilities.Cors;
using DDDTemplateServices.Insfrastructure.Utilities.Exceptions.GlobalEror;
using DDDTemplateServices.Insfrastructure.Utilities.Identity;
using DDDTemplateServices.Insfrastructure.Utilities.Identity.Middleware;
using DDDTemplateServices.Insfrastructure.Utilities.Ioc;
using DDDTemplateServices.Insfrastructure.Utilities.Logging;
using DDDTemplateServices.Insfrastructure.Utilities.MediatR;
using DDDTemplateServices.Insfrastructure.Utilities.ServiceBus;
using DDDTemplateServices.Persistence.Context;
using DDDTemplateServices.Persistence.Extensions;
using DDDTemplateServices.Persistence.GenericRepository;
using FluentValidation;
using Prometheus;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);
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
//Build
var app = builder.Build();
//WebUI
app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    //Metrics
    app.UseHttpMetrics();
    app.MapMetrics();
    app.UseHttpsRedirection();
}
//Core
app.UseCors();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
//MiddleWare
await app.GenerateDbRole(Assembly.GetExecutingAssembly());
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ClaimMiddleware>();
ServiceTool.ServiceProvider = app.Services;
app.Run();
