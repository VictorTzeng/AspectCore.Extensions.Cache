﻿using System;
using Microsoft.Extensions.Caching.Memory;

namespace AspectCore.Extensions.Cache
{
    [Obsolete]
    public class MemoryCacheManager
    {
        public static IMemoryCache GetInstance() => ServiceLocator.Resolve<IMemoryCache>();
    }
}
