# Introduction
A redis & memory cache middleware using [AspectCore-Framework](https://github.com/dotnetcore/AspectCore-Framework) & [csredis](https://github.com/2881099/csredis).

# How to use
* 1. configure services in StartUp.cs:
```
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            
            services.AddAspectCoreMemoryCache();
        
            services.AddAspectCoreRedisCache("your redis connetion string");
            
            return services.BuildServiceProvider();
        }
```
* 2. declare an inteface.

    CacheKey -> default value : {namespace}{class}{method}{params hashcode}

    Expiration -> default value : 600 (seconds)

```
    public interface ISysMenuRepository:IRepository<SysMenu, string>
    {
        [Cached(CacheKey = "Redis_Cache_SysMenu", Expiration = 5)]
        IList<SysMenu> GetMenusByCache(Expression<Func<SysMenu, bool>> where);
    }
```
* 3. declare an implement class
```
    public class SysMenuRepository : BaseRepository<SysMenu, string>, ISysMenuRepository
    {
        public IList<SysMenu> GetMenusByCache(Expression<Func<SysMenu, bool>> @where)
        {
            return DbContext.Get(where, true).ToList();
        }
    }
```
