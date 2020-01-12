using System;
using System.Threading.Tasks;

namespace apigw.Cache
{
    public interface ICache<T>
    {
        Task<CacheValue<T>> Get(string cacheKey, Func<int, Task<T>> valueResolver);
        Task Clear(string cacheKey);
    }
}
