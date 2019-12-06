[![Build Status](https://dev.azure.com/v-xiaze0473/v-xiaze/_apis/build/status/VictorTzeng.AspectCore.Extensions.Cache?branchName=master)](https://dev.azure.com/v-xiaze0473/v-xiaze/_build/latest?definitionId=3&branchName=master)

# Introduction
A redis & memory cache middleware using [AspectCore-Framework](https://github.com/dotnetcore/AspectCore-Framework) & [csredis](https://github.com/2881099/csredis).

# How to use

* In dotnetcore 3.0, we must configure Program.cs:
```
using AspectCore.Extensions.DependencyInjection;// import this namespace
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            // for aspcectcore
            .UseServiceProviderFactory(new AspectCoreServiceProviderFactory())
        ;
}
```

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
