using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCore.Extensions.Cache
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddAspectCoreRedisCache(this IServiceCollection services,
            params string[] redisConnectionStrings)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (redisConnectionStrings == null || redisConnectionStrings.Length == 0)
            {
                throw new ArgumentNullException(nameof(redisConnectionStrings));
            }
            CSRedisClient redisClient;
            if (redisConnectionStrings.Length == 1)
            {
                //单机模式
                redisClient = new CSRedisClient(redisConnectionStrings[0]);
            }
            else
            {
                //集群模式
                redisClient = new CSRedisClient(NodeRule: null, connectionStrings: redisConnectionStrings);
            }
            //初始化 RedisHelper
            RedisHelper.Initialization(redisClient);
            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            return services;
        }

        public static IServiceCollection AddAspectCoreMemoryCache(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.AddMemoryCache();
        }
    }
}
