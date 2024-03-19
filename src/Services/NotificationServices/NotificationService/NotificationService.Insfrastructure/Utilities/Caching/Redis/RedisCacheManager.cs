using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;

namespace NotificationService.Insfrastructure.Utilities.Caching.Redis
{
    /// <summary>
    /// use redis for cache
    /// </summary>
    public class RedisCacheManager(IDistributedCache distributedCache, IConfiguration configuration) : ICacheService
    {
        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly IConfiguration _configuration = configuration;

        public string GetKey(string region, string methodName, object arg)
        {
            return $"{region}:{methodName}({BuildKey(arg)})";
        }
        public string? Get(string key)
        {
            return _distributedCache.GetString(key);
        }
        public void Set(string key, string value)
        {
            _distributedCache.SetString(key, value, GetDistributedCacheEntryOptions());
        }
        public async Task<string?> GetAsync(string key, CancellationToken cancellation = default)
        {
            return await _distributedCache.GetStringAsync(key, cancellation);
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellation = default)
            where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync(key, cancellation);
            if (cachedValue is null)
            {
                return null;
            }
            T? getValue = JsonConvert.DeserializeObject<T>(cachedValue);
            return getValue;
        }
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellation = default)
           where T : class
        {
            T? cachedValue = await GetAsync<T>(key, cancellation);
            if (cachedValue is not null)
            {
                return (T)cachedValue;
            }
            cachedValue = await factory();
            await SetAsync(key, cachedValue, cancellation);
            return (T)cachedValue;
        }
        public async Task<T> GetAsync<T>(IRequest<T> arg, Func<Task<T>> factory, CancellationToken cancellation = default)
         where T : class
        {
            var region = _configuration["RegionName"];
            var methodName = arg.GetType().FullName?.Replace(region + ".Application.Handlers.", "");
            var key = $"{region}:{methodName}({BuildKey(arg)})";
            T? cachedValue = await GetAsync<T>(key, cancellation);
            if (cachedValue is not null)
            {
                return (T)cachedValue;
            }
            cachedValue = await factory();
            await SetAsync(key, cachedValue, cancellation);
            return (T)cachedValue;
        }
        public async Task SetAsync<T>(string key, T value, CancellationToken cancellation = default)
          where T : class
        {
            string? cachedValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, cachedValue, GetDistributedCacheEntryOptions(), cancellation);
            CacheKeys.TryAdd(key, false);
        }
        public async Task RemovePatternAsync(string key, CancellationToken cancellation = default)
        {
            await _distributedCache.RemoveAsync(key, cancellation);
            CacheKeys.TryRemove(key, out bool _);
        }
        public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellation = default)
        {
            var tasks = CacheKeys.Keys
                .Where(x => x.StartsWith(prefixKey))
                .Select(y => RemovePatternAsync(y, cancellation));
            await Task.WhenAll(tasks);
        }
        private static string? BuildKey(object arg)
        {
            if (arg != null)
            {
                var sb = new StringBuilder();
                var paramValues = arg.GetType()
                    .GetProperties()
                    .Select(p => p.GetValue(arg)?.ToString() ?? string.Empty);
                sb.AppendJoin('_', paramValues);
                return sb.ToString();
            }
            return null;
        }
        private static DistributedCacheEntryOptions GetDistributedCacheEntryOptions(int ttlSecond = 60)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttlSecond)
            };
        }
    }
}
