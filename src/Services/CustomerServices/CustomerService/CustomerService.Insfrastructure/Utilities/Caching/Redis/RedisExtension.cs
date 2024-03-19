using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CustomerService.Insfrastructure.Utilities.Caching.Redis
{
    /// <summary>
    /// Add redis to webapp
    /// </summary>
    public static class RedisExtension
    {
        public static WebApplicationBuilder AddRedis(this WebApplicationBuilder builder)
        {
            var conf = builder.Configuration.GetSection("Redis").Get<RedisSetting>();
            if (conf != null)
            {
                builder.Services.AddTransient<ICacheService, RedisCacheManager>();
                builder.Services.AddStackExchangeRedisCache(option =>
                {
                    option.ConfigurationOptions = new ConfigurationOptions
                    {
                        EndPoints = { { conf.Host, conf.Port } },
                        User = conf.User,
                        Password = conf.Password,
                        Ssl = conf.Ssl,
                        SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                    };
                });
            }
            return builder;
        }
    }
}
