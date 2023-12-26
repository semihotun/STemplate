using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
namespace DDDTemplateServices.Persistence.Context
{
    /// <summary>
    /// Some Db context migrate with polly
    /// </summary>
    public class CoreDbContextSeed
    {
        public async Task SeedAsync(CoreDbContext context,
            ILogger<CoreDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(CoreDbContextSeed));
            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    context.Database.Migrate();
                    await Task.CompletedTask;
                }
            });
        }
        private AsyncRetryPolicy CreatePolicy(ILogger<CoreDbContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => System.TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning("Exception message");
                });
        }
    }
}
