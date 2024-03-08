using Nest;
using STemplate.Domain.SeedWork;

namespace STemplate.Persistence.SearchEngine;

public interface ICoreSearchEngineContext
{
    ElasticClient Client { get; }
    string IndexName<T>() where T : IElasticEntity;
}
