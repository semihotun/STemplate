using Carter;
using CatalogService.Application.Assemblies;
using CatalogService.Application.Jobs;
using CatalogService.Extensions;
using CatalogService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using CatalogService.Insfrastructure.Utilities.HangFire;
using CatalogService.Insfrastructure.Utilities.Identity.Middleware;
using CatalogService.Insfrastructure.Utilities.Ioc;
using CatalogService.Insfrastructure.Utilities.Outboxes;
using Hangfire;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
await builder.AddStartupServicesAsync();
var app = builder.Build();
GlobalConfiguration.Configuration.UseActivator(new HangfireJobActivator(app.Services));
app.MapCarter();
if (!app.Environment.IsDevelopment())
{
    //Metrics
    app.UseHttpMetrics();
    app.MapMetrics();
    app.UseHttpsRedirection();
}
//WebUI
app.UseSwagger();
app.UseSwaggerUI();
//Core
app.UseCors();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
//MiddleWare
ServiceTool.ServiceProvider = app.Services;
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ClaimMiddleware>();
var task = app.RunAsync();
await app.AddOutboxKafkaConsumerAsync(ApplicationAssemblyExtension.GetApplicationAssembly());
_ = app.GenerateDbRole();
HangFireJobs.AddAllStartupJobs();
await task;