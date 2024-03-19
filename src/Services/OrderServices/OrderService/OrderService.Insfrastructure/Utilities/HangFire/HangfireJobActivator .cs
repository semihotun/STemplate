using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Insfrastructure.Utilities.HangFire
{
    public class HangfireJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;
        public HangfireJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override object ActivateJob(Type jobType)
        {
            return _serviceProvider.GetRequiredService(jobType);
        }
    }
}
