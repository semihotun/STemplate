using Nest;
using ShippingService.Domain.SeedWork;

namespace ShippingService.Persistence.SearchEngine
{
    public interface ICoreSearchEngineContext
    {
        ElasticClient Client { get; }
        string IndexName<T>() where T : IElasticEntity;
    }
}
