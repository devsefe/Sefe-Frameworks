using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCache = System.Runtime.Caching;

namespace Sefe.Caching
{
    /// <summary>
    /// Doing memory cache in Business layer
    /// </summary>
    public static class CacheManager
    {
        static readonly DataCache.MemoryCache cache = DataCache.MemoryCache.Default;

        /// <summary>
        /// Doing MemoryCaching
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Cache value</param>
        /// <param name="cacheTime">Cache time (in minute)</param>
        public static void AddToCache(string key, object value, int cacheTime)
        {
            cache.Add(key, value, DateTime.Now.AddMinutes(cacheTime));
        }
        /// <summary>
        /// Getting value from cache
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Cahce key</param>
        /// <returns></returns>
        public static T GetFromCache<T>(string key) where T : class
        {
            try
            {
                return (T)cache[key];
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Getting value from cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns></returns>
        public static object GetFromCache(string key)
        {
            return cache[key];
        }
        /// <summary>
        /// Deleting value from cache
        /// </summary>
        /// <param name="key">Cache key</param>
        public static void ClearCache(string key)
        {
            cache.Remove(key);
        }
        /// <summary>
        /// Delete all cache values that includes given parameter
        /// Example: UserRoleRight_1_2_2017, UserRoleRight_2_3_2019 key parameter is "UserRoleRight". Seek given parameter in cache then delete.
        /// </summary>
        /// <param name="key"></param>
        public static void ClearCacheFromLikeKey(string key)
        {
            List<string> cacheKeys = cache.Where(w => w.Key.Contains(key)).Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                cache.Remove(cacheKey);
            }
            cache.Remove(key);
        }
        /// <summary>
        /// Generate cache Key
        /// Example key: EmployeesBySearchParams_1_3_2017
        /// </summary>
        /// <param name="entityType">Any entity name or any module name (or some string)</param>
        /// <param name="args">Parameters</param>
        /// <returns></returns>
        public static string GenerateCacheKey(string entityType, params object[] args)
        {
            return string.Format("{0}_{1}", entityType, string.Join("_", args));
        }
    }
}
