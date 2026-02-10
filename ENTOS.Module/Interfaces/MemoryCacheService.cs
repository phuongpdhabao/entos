using ENTOS.Module.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ENTOS.Module.SystemServices
{
    /// <summary>
    /// Triển khai cache service sử dụng Memory cache
    /// Thích hợp cho cache dữ liệu trong memory của ứng dụng
    /// </summary>
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memory;
        private readonly ILogger<MemoryCacheService> _logger;

        public MemoryCacheService(IMemoryCache memory, ILogger<MemoryCacheService> logger = null)
        {
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _logger = logger;
        }

        /// <inheritdoc />
        public T GetOrCreate<T>(string key, Func<T> factory, int cacheMinutes = 5)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Cache key cannot be null or empty", nameof(key));

                return _memory.GetOrCreate(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMinutes);
                    entry.Priority = CacheItemPriority.Normal;
                    return factory();
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in GetOrCreate for key: {Key}", key);
                return factory(); // Fallback to direct execution
            }
        }

        /// <inheritdoc />
        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createFunc, int cacheMinutes = 5)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Cache key cannot be null or empty", nameof(key));

                return await _memory.GetOrCreateAsync(key, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMinutes);
                    entry.Priority = CacheItemPriority.Normal;
                    return await createFunc();
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in GetOrCreateAsync for key: {Key}", key);
                return await createFunc(); // Fallback to direct execution
            }
        }

        /// <inheritdoc />
        public void Set<T>(string key, T value, int cacheMinutes = 5)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Cache key cannot be null or empty", nameof(key));

                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMinutes),
                    Priority = CacheItemPriority.Normal
                };

                _memory.Set(key, value, options);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error setting cache for key: {Key}", key);
            }
        }

        /// <inheritdoc />
        public Task SetAsync<T>(string key, T value, int cacheMinutes = 5)
        {
            Set(key, value, cacheMinutes);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public bool TryGet<T>(string key, out T value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    value = default;
                    return false;
                }

                return _memory.TryGetValue(key, out value);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in TryGet for key: {Key}", key);
                value = default;
                return false;
            }
        }

        /// <inheritdoc />
        public Task<(bool found, T value)> TryGetAsync<T>(string key)
        {
            var found = TryGet<T>(key, out var value);
            return Task.FromResult((found, value));
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    _memory.Remove(key);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error removing cache for key: {Key}", key);
            }
        }

        /// <inheritdoc />
        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }
    }
}
