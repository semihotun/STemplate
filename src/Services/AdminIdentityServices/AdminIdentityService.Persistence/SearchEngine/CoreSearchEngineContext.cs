using AdminIdentityService.Domain.SeedWork;
using Microsoft.Extensions.Configuration;
using Nest;
using System.Globalization;
using ConnectionSettings = Nest.ConnectionSettings;

namespace AdminIdentityService.Persistence.SearchEngine;

public class CoreSearchEngineContext : ICoreSearchEngineContext
{
    public ElasticClient Client { get; }
    public CoreSearchEngineContext(IConfiguration configuration)
    {
        Client = new ElasticClient(new ConnectionSettings(new Uri(configuration["ElasticConfiguration:Uri"]!)));
    }
    public string IndexName<T>() where T : IElasticEntity
    {
        return ("AdminIdentityService-" + typeof(T).Name).ToLower(new CultureInfo("en-US"));
    }
}