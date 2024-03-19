using Carter;
using Hangfire;
using NotificationService.Application.Assemblies;
using NotificationService.Application.Jobs;
using NotificationService.Extensions;
using NotificationService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using NotificationService.Insfrastructure.Utilities.HangFire;
using NotificationService.Insfrastructure.Utilities.Identity.Middleware;
using NotificationService.Insfrastructure.Utilities.Ioc;
using NotificationService.Insfrastructure.Utilities.Outboxes;
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