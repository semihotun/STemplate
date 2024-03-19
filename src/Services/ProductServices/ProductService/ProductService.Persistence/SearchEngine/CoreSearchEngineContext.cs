using Microsoft.Extensions.Configuration;
using Nest;
using ProductService.Domain.SeedWork;
using System.Globalization;

namespace ProductService.Persistence.SearchEngine
{
    public class CoreSearchEngineContext : ICoreSearchEngineContext
    {
        public ElasticClient Client { get; }
        public CoreSearchEngineContext(IConfiguration configuration)
        {
            Client = new ElasticClient(new ConnectionSettings(new Uri(configuration["ElasticConfiguration:Uri"]!)));
        }
        public string IndexName<T>() where T : IElasticEntity
        {
            return ("ProductService-" + typeof(T).Name).ToLower(new CultureInfo("en-US"));
        }
    }
}
