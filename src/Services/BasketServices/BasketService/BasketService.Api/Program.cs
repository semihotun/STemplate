using BasketService.Application.Assemblies;
using BasketService.Application.Jobs;
using BasketService.Extensions;
using BasketService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using BasketService.Insfrastructure.Utilities.HangFire;
using BasketService.Insfrastructure.Utilities.Identity.Middleware;
using BasketService.Insfrastructure.Utilities.Ioc;
using BasketService.Insfrastructure.Utilities.Outboxes;
using Carter;
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