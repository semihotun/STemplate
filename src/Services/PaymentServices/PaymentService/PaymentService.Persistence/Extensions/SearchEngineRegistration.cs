using Microsoft.Extensions.Configuration;
using Nest;
using PaymentService.Domain.SeedWork;
using System.Globalization;
using System.Reflection;

namespace PaymentService.Persistence.Extensions
{
    public static class SearchEngineRegistration
    {
        public static ElasticClient GetElasticClient(IConfiguration configuration)
        {
            return new ElasticClient(
            new ConnectionSettings(
            new Uri(configuration["ElasticConfiguration:Uri"] ?? "http://host.docker.internal:9200")));
        }
        public static async Task MigrateElasticDbAsync(Assembly[] assemblies, IConfiguration configuration)
        {
            var entityTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IElasticEntity).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();
            var client = GetElasticClient(configuration);
            await Task.WhenAll(entityTypes.Select(async entity =>
            {
                var name = $"{configuration["RegionName"]}-{entity.Name}".ToLower(new CultureInfo("en-US"));
                if (!(await client.Indices.ExistsAsync(name)).Exists)
                {
                    var response = await client.Indices.CreateAsync(name
                    , se => se.Settings(a => a.NumberOfReplicas(3).NumberOfShards(3)));
                }
            }));
        }
    }
}
