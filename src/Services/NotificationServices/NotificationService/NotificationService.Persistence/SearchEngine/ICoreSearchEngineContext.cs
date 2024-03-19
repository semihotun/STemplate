using Nest;
using NotificationService.Domain.SeedWork;

namespace NotificationService.Persistence.SearchEngine
{
    public interface ICoreSearchEngineContext
    {
        ElasticClient Client { get; }
        string IndexName<T>() where T : IElasticEntity;
    }
}
