using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentService.Persistence.Constant;
using Polly;
using Polly.Retry;

namespace PaymentService.Persistence.Context
{
    /// <summary>
    /// Some Db context migrate with polly
    /// </summary>
    public class CoreDbContextSeed
    {
        public static async Task SeedAsync(CoreDbContext context,
            ILogger<CoreDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger);
            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    context.Database.Migrate();
                    await Task.CompletedTask;
                }
            });
        }
        private static AsyncRetryPolicy CreatePolicy(ILogger<CoreDbContextSeed> logger, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: _ => System.TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry) => logger.LogWarning(PersistenceConstant.ExceptionMessageTemplate + exception.Message + timeSpan + retry));
        }
    }
}
