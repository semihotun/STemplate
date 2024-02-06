using Carter;
using Prometheus;
using STemplate.Extensions;
using STemplate.Insfrastructure.Utilities.Exceptions.GlobalEror;
using STemplate.Insfrastructure.Utilities.Identity.Middleware;
using STemplate.Insfrastructure.Utilities.Ioc;
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
app.UseAuthorization();
app.MapControllers();
//MiddleWare
ServiceTool.ServiceProvider = app.Services;
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ClaimMiddleware>();
var task = app.RunAsync();
_ = app.GenerateDbRole();
await task;