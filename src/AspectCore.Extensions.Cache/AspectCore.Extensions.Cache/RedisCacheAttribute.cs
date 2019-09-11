﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Newtonsoft.Json;

namespace AspectCore.Extensions.Cache
{
    /// <summary>
    /// 缓存属性。
    /// <para>
    /// 在方法上标记此属性后，通过该方法取得的数据将被缓存。在缓存有效时间范围内，往后通过此方法取得的数据都是从缓存中取出的。
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RedisCacheAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 缓存有限期，单位：秒。默认值：600。
        /// </summary>
        public int Expiration { get; set; } = 10 * 60;

        public string CacheKey { get; set; } = null;

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var parameters = context.ServiceMethod.GetParameters();
            //判断Method是否包含ref / out参数
            if (parameters.Any(it => it.IsIn || it.IsOut))
            {
                await next(context);
            }
            else
            {
                var key = string.IsNullOrEmpty(CacheKey)
                    ? new CacheKey(context.ServiceMethod, parameters, context.Parameters).GetRedisCacheKey()
                    : CacheKey;
                var value =  await DistributedCacheManager.GetAsync<object>(key);
                if (value != null)
                {
                    if (context.ServiceMethod.IsReturnTask())
                    {
                        context.ReturnValue = Task.FromResult(value);
                    }
                    else
                    {
                        context.ReturnValue = value;
                    }
                }
                else
                {
                    await context.Invoke(next);
                    dynamic returnValue = context.ReturnValue;
                    if (context.ServiceMethod.IsReturnTask())
                    {
                        returnValue = returnValue.Result;
                    }
                    await DistributedCacheManager.SetAsync(key, returnValue, Expiration);
                }
            }
        }
    }
}
