using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace TimeChimp.Backend.Assessment.Extensions
{
    public static class CacheExtensions
    {
        public static async Task<T> GetCacheValueAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            string result = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            var deserializedObj = JsonConvert.DeserializeObject<T>(result);
            return deserializedObj;
        }

        public static async Task SetCacheValueAsync<T>(this IDistributedCache cache, string key, T value) where T : class
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
            cacheEntryOptions.SlidingExpiration = TimeSpan.FromSeconds(30);

            string result = JsonConvert.SerializeObject(value);
            await cache.SetStringAsync(key, result);
        }
    }
}
