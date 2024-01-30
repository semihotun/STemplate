using STemplate.Extensions;
using STemplate.Insfrastructure.Utilities.Exceptions.GlobalEror;
using STemplate.Insfrastructure.Utilities.Identity.Middleware;
using STemplate.Insfrastructure.Utilities.Ioc;
using Prometheus;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);
builder.AddStartupServices();
var app = builder.Build();
//WebUI
app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    //Metrics
    app.UseHttpMetrics();
    app.MapMetrics();
    app.UseHttpsRedirection();
}
//Core
app.UseCors();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
//MiddleWare
await app.GenerateDbRole(Assembly.GetExecutingAssembly());
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ClaimMiddleware>();
ServiceTool.ServiceProvider = app.Services;
app.Run();
