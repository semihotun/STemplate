using Carter;
using Hangfire;
using Prometheus;
using ShippingService.Application.Assemblies;
using ShippingService.Application.Jobs;
using ShippingService.Extensions;
using ShippingService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using ShippingService.Insfrastructure.Utilities.HangFire;
using ShippingService.Insfrastructure.Utilities.Identity.Middleware;
using ShippingService.Insfrastructure.Utilities.Ioc;
using ShippingService.Insfrastructure.Utilities.Outboxes;

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