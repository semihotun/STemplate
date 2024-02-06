using Ocelot.Middleware;
using Web.ApiGateway.Extension;
var builder = WebApplication.CreateBuilder(args);
builder.AddStartupServices();
var app = builder.Build();
app.GetHashCode();
//ApiDocs
app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
//Core
app.UseCors();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
await app.UseOcelot();
app.Run();
