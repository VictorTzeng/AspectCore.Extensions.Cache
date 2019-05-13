using System;
using System.Collections.Generic;
using System.Linq;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCore.Extensions.Cache
{
    public static class ServiceLocator
    {
        private static IServiceResolver Resolver { get; set; }
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services, Action<IAspectConfiguration> configure = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.ConfigureDynamicProxy(configure);
            services.AddAspectCoreContainer();
            return Resolver = services.ToServiceContainer().Build();
        }

        public static T Resolve<T>()
        {
            if (Resolver == null)
                throw new ArgumentNullException(nameof(Resolver), "调用此方法前必须先调用BuildServiceProvider！");
            return Resolver.Resolve<T>();
        }
        public static List<T> ResolveServices<T>()
        {
            if (Resolver == null)
                throw new ArgumentNullException(nameof(Resolver), "调用此方法前必须先调用BuildServiceProvider！");
            return Resolver.GetServices<T>().ToList();
        }
    }
}
