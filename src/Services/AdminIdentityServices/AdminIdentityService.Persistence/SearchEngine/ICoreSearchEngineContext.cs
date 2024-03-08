using AdminIdentityService.Domain.SeedWork;
using Nest;

namespace AdminIdentityService.Persistence.SearchEngine;

public interface ICoreSearchEngineContext
{
    ElasticClient Client { get; }
    string IndexName<T>() where T : IElasticEntity;
}
