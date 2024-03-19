using Carter;
using Hangfire;
using PaymentService.Application.Assemblies;
using PaymentService.Application.Jobs;
using PaymentService.Extensions;
using PaymentService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using PaymentService.Insfrastructure.Utilities.HangFire;
using PaymentService.Insfrastructure.Utilities.Identity.Middleware;
using PaymentService.Insfrastructure.Utilities.Ioc;
using PaymentService.Insfrastructure.Utilities.Outboxes;
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