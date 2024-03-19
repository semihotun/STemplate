using Carter;
using CustomerService.Application.Assemblies;
using CustomerService.Application.Jobs;
using CustomerService.Extensions;
using CustomerService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using CustomerService.Insfrastructure.Utilities.HangFire;
using CustomerService.Insfrastructure.Utilities.Identity.Middleware;
using CustomerService.Insfrastructure.Utilities.Ioc;
using CustomerService.Insfrastructure.Utilities.Outboxes;
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