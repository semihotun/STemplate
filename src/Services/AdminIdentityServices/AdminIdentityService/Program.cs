using AdminIdentityService.Extensions;
using AdminIdentityService.Insfrastructure.Utilities.Exceptions.GlobalEror;
using AdminIdentityService.Insfrastructure.Utilities.Identity.Middleware;
using AdminIdentityService.Insfrastructure.Utilities.Ioc;
using Carter;
using Prometheus;
var builder = WebApplication.CreateBuilder(args);
builder.AddStartupServices();
var app = builder.Build();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//MiddleWare
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ClaimMiddleware>();
ServiceTool.ServiceProvider = app.Services;
var task = app.RunAsync();
_= app.GenerateDbRole();
await task;
