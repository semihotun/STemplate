using Carter;
using Hangfire;
using Prometheus;
using SearchBarService.Application.Assemblies;
using SearchBarService.Application.Jobs;
using SearchBarService.Extensions;
using SearchBarService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using SearchBarService.Insfrastructure.Utilities.HangFire;
using SearchBarService.Insfrastructure.Utilities.Identity.Middleware;
using SearchBarService.Insfrastructure.Utilities.Ioc;
using SearchBarService.Insfrastructure.Utilities.Outboxes;

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