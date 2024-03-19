using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingService.Persistence.Cdc.MssqlContext;
using ShippingService.Persistence.Context;

namespace ShippingService.Persistence.Extensions
{
    public static class DbContextRegistiration
    {
        /// <summary>
        /// Add DbContext
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static async Task<IServiceCollection> ConfigureDbContextAsync(this IServiceCollection services, IConfiguration configuration)
        {
            var regionName = configuration["RegionName"] + "SqlConnectionString";
            var connectionString = configuration[regionName];
            services.AddEntityFrameworkSqlServer().AddDbContext<CoreDbContext>(option =>
            {
                option.UseSqlServer(connectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 15,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });
            var optionBuilder = new DbContextOptionsBuilder<CoreDbContext>()
                .UseSqlServer(connectionString);
            await using (var ctx = new CoreDbContext(optionBuilder.Options))
            {
                if (ctx.Database.EnsureCreated())
                {
                    ctx.Database.Migrate();
                    CreateCdcForOutboxWithMssql(ctx);
                    await MssqlDbContextConnectorExtension.AddAllConnectorAsync(configuration);
                }
                else
                {
                    ctx.Database.EnsureCreated();
                    ctx.Database.Migrate();
                }
            }
            return services;
        }
        private static void CreateCdcForOutboxWithMssql(CoreDbContext ctx)
        {
            ctx.Database.ExecuteSqlRaw("EXEC sys.sp_cdc_enable_db;");
            MssqlDbContextAddAllCdcTable.AddAllCdc(ctx);
        }
    }
}
