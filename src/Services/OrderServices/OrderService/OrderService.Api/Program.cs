using Carter;
using Hangfire;
using OrderService.Application.Assemblies;
using OrderService.Application.Jobs;
using OrderService.Extensions;
using OrderService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using OrderService.Insfrastructure.Utilities.HangFire;
using OrderService.Insfrastructure.Utilities.Identity.Middleware;
using OrderService.Insfrastructure.Utilities.Ioc;
using OrderService.Insfrastructure.Utilities.Outboxes;
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