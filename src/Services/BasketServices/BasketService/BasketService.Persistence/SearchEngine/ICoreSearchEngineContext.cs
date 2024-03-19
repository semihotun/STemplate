using BasketService.Domain.SeedWork;
using Nest;

namespace BasketService.Persistence.SearchEngine
{
    public interface ICoreSearchEngineContext
    {
        ElasticClient Client { get; }
        string IndexName<T>() where T : IElasticEntity;
    }
}
