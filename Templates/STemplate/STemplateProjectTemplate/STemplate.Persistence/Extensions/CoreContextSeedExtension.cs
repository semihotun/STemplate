using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Serilog;
using STemplate.Persistence.Context;
using System.Data.SqlClient;
using System.Reflection;
namespace STemplate.Persistence.Extensions
{
    public static class CoreContextSeedExtension
    {
        /// <summary>
        /// Core Context Seed
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task SeedAsync(this CoreDbContext context)
        {
            var policy = CreatePolicy(3);
            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    var assm = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterfaces().Any(x => x.Name == "ISeed`1"));
                    foreach (var item in assm)
                    {
                        var method = item.GetMethod("GetSeedData");
                        var obj = Activator.CreateInstance(item);
                        var dataObjects = (IEnumerable<object>)method?.Invoke(obj, null)!;
                        context.AddRange(dataObjects);
                    }
                    await context.SaveChangesAsync();
                }
            });
        }
        /// <summary>
        /// Polly Retry Mechanism
        /// </summary>
        /// <param name="retries"></param>
        /// <returns></returns>
        private static AsyncRetryPolicy CreatePolicy(int retries)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(retry),
                    onRetry: (exception, _, _, _) => Log.Warning(exception.Message)
               );
        }
    }
}
