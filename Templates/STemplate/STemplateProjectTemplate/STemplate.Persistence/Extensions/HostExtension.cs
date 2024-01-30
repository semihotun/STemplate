using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using STemplate.Persistence.Constant;
namespace STemplate.Persistence.Extensions
{
    public static class HostExtension
    {
        /// <summary>
        /// Migrate dbcontext with mssql
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="host"></param>
        /// <param name="seeder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder MigrateDbContext<TContext>(this WebApplicationBuilder host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
        {
            var services = host.Services.BuildServiceProvider();
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();
            try
            {
                var rety = Policy.Handle<System.Data.SqlClient.SqlException>()
                    .WaitAndRetry(new TimeSpan[]
                    {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                    });
                rety.Execute(() => InvokeSeeder(seeder, context, services));
                logger.LogInformation(PersistenceConstant.MigratedDbContext+ typeof(TContext).Name);
            }
            catch (Exception)
            {
                logger.LogError(PersistenceConstant.MigrationError + typeof(TContext).Name);
            }
            return host;
        }
        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
