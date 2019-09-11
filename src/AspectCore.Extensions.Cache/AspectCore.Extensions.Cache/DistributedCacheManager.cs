using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AspectCore.Extensions.Cache
{
    public class DistributedCacheManager
    {
        public static async Task<T> GetAsync<T>(string key)
        {
            if (await RedisHelper.ExistsAsync(key))
            {
                var content = await RedisHelper.GetAsync<T>(key);
                return content;
            }

            return default(T);
        }

        public static T Get<T>(string key)
        {
            if(RedisHelper.Exists(key))
                return RedisHelper.Get<T>(key);
            return default(T);
        }


        public static void Set(string key, object data, int expiredSeconds)
        {
            RedisHelper.Set(key, data, expiredSeconds);
        }

        public static async Task<bool> SetAsync(string key, object data, int expiredSeconds)
        {
            return await RedisHelper.SetAsync(key, data, expiredSeconds);
        }
    }
}
