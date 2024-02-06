using Ocelot.Values;

namespace Web.ApiGateway.Extension
{
    public static class StartUpExtension
    {
        public static void AddStartupServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
                policy.WithOrigins($"https://{builder.Configuration["HostAdress"]}:4000",
                                          $"http://{builder.Configuration["HostAdress"]}:4000")
                .AllowAnyHeader().AllowAnyMethod().AllowCredentials()
            ));
            builder.Services.AddHttpClient();
            if (builder.Environment.IsDevelopment())
            {
                builder.AddOcelotDevelopmentSetting();
            }
            else
            {
                builder.AddOcelotProductionSetting();
            }
        }
    }
}
