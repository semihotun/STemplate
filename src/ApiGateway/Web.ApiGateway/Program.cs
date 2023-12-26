using Ocelot.Middleware;
using Web.ApiGateway.Extension;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//Ocelot
builder.AddOcelotSetting();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:5000", "http://localhost:5001").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));
builder.Services.AddHttpClient();
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
