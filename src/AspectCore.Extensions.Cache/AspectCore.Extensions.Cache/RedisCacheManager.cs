using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AspectCore.Extensions.Cache
{
    [Obsolete]
    public class RedisCacheManager
    {
        private static IDistributedCache Instance => ServiceLocator.Resolve<IDistributedCache>();

        public static string Get(string key)
        {
            if (RedisHelper.Exists(key))
            {
                return RedisHelper.Get(key);
            }

            return null;
        }

        public static async Task<string> GetAsync(string key)
        {
            if (await RedisHelper.ExistsAsync(key))
            {
                var content = await RedisHelper.GetAsync(key);
                return content;
            }

            return null;
        }

        public static T Get<T>(string key)
        {
            var value = Get(key);
            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);
            return default(T);
        }

        public static async Task<T> GetAsync<T>(string key)
        {
            var value = await GetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default(T);
        }

        public static void Set(string key, object data, int expiredSeconds)
        {
            RedisHelper.Set(key, JsonConvert.SerializeObject(data), expiredSeconds);
        }

        public static async Task<bool> SetAsync(string key, object data, int expiredSeconds)
        {
            return await RedisHelper.SetAsync(key, JsonConvert.SerializeObject(data), expiredSeconds);
        }


        public static void Remove(string key) => Instance.Remove(key);

        public static async Task RemoveAsync(string key) => await Instance.RemoveAsync(key);

        public static void Refresh(string key) => Instance.Refresh(key);

        public static async Task RefreshAsync(string key) => await Instance.RefreshAsync(key);

        public static void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
