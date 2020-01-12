using System;
using System.Threading.Tasks;
using EasyCaching.Core;

namespace apigw.Cache
{
    public class SimpleCache<T> : ICache<T>
    {
        private readonly IEasyCachingProvider _cache;

        public SimpleCache(IEasyCachingProviderFactory cacheProviderFactory)
        {
            _cache = cacheProviderFactory.GetCachingProvider("cache");
        }

        public async Task<CacheValue<T>> Get(string cacheKey, Func<int, Task<T>> valueResolver)
        {
            var fullCacheKey = CreateCacheKey(cacheKey);
            T value;

            var cachedValue = await _cache.GetAsync<T>(fullCacheKey);
            value = cachedValue.Value;

            if (!cachedValue.HasValue)
            {
                value = await valueResolver(1);

                await _cache.TrySetAsync<T>(fullCacheKey, value, TimeSpan.FromMinutes(1));
            }

            return new CacheValue<T>(value);
        }

        public async Task Clear(string cacheKey)
        {
            await _cache.RemoveAsync(CreateCacheKey(cacheKey));
        }

        private string CreateCacheKey(string ext)
        {
            return typeof(T) + "." + ext;
        }
    }
}
