using MediatR;

namespace ShippingService.Insfrastructure.Utilities.Caching.Redis
{
    /// <summary>
    /// Cache interface
    /// </summary>
    public interface ICacheService
    {
        string? Get(string key);
        void Set(string key, string value);
        string GetKey(string region, string methodName, object arg);
        Task<string?> GetAsync(string key, CancellationToken cancellation = default);
        Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellation = default) where T : class;
        Task<T> GetAsync<T>(IRequest<T> arg, Func<Task<T>> factory, CancellationToken cancellation = default) where T : class;
        Task SetAsync<T>(string key, T value, CancellationToken cancellation = default) where T : class;
        Task RemovePatternAsync(string key, CancellationToken cancellation = default);
        Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellation = default);
    }
}
