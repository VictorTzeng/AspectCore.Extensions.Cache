using Microsoft.Extensions.Caching.Memory;

namespace AspectCore.Extensions.Cache
{
    public class MemoryCacheManager
    {
        public static IMemoryCache GetInstance() => ServiceLocator.Resolve<IMemoryCache>();
    }
}
