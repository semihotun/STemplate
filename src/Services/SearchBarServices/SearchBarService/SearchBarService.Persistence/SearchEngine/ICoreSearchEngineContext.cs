using Nest;
using SearchBarService.Domain.SeedWork;

namespace SearchBarService.Persistence.SearchEngine
{
    public interface ICoreSearchEngineContext
    {
        ElasticClient Client { get; }
        string IndexName<T>() where T : IElasticEntity;
    }
}
