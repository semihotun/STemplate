using AdminIdentityService.Application.Assemblies;
using AdminIdentityService.Application.Extension;
using AdminIdentityService.Extensions;
using AdminIdentityService.Insfrastructure.Utilities.Caching.Redis;
using AdminIdentityService.Insfrastructure.Utilities.Cors;
using AdminIdentityService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using AdminIdentityService.Insfrastructure.Utilities.Identity;
using AdminIdentityService.Insfrastructure.Utilities.Identity.Middleware;
using AdminIdentityService.Insfrastructure.Utilities.Ioc;
using AdminIdentityService.Insfrastructure.Utilities.Logging;
using AdminIdentityService.Insfrastructure.Utilities.MediatR;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using AdminIdentityService.Insfrastructure.Utilities.ServiceBus;
using AdminIdentityService.Persistence.Context;
using AdminIdentityService.Persistence.Extensions;
using AdminIdentityService.Persistence.GenericRepository;
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
builder.Services.AddTransient<ITokenService, TokenService>();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//MiddleWare
await app.GenerateDbRole(Assembly.GetExecutingAssembly());
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ClaimMiddleware>();
ServiceTool.ServiceProvider = app.Services;
app.Run();
