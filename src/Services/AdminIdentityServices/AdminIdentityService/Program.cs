using AdminIdentityService.Application.Assemblies;
using AdminIdentityService.Application.Jobs;
using AdminIdentityService.Extensions;
using AdminIdentityService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using AdminIdentityService.Insfrastructure.Utilities.Hangfire;
using AdminIdentityService.Insfrastructure.Utilities.Identity.Middleware;
using AdminIdentityService.Insfrastructure.Utilities.Ioc;
using AdminIdentityService.Insfrastructure.Utilities.Outboxes;
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
app.UseHttpMetrics();
app.MapMetrics();
app.UseHttpsRedirection();
//WebUI
app.UseSwagger();
app.UseSwaggerUI();
//Core
app.UseCors();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//MiddleWare
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ClaimMiddleware>();
ServiceTool.ServiceProvider = app.Services;
var task = app.RunAsync();
_= app.GenerateDbRole();
HangFireJobs.AddAllStartupJobs();
await app.AddOutboxKafkaConsumerAsync(ApplicationAssemblyExtension.GetApplicationAssembly());
await task;
