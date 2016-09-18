using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Stashbox.Infrastructure;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace WindowsAuthenticationSample.DependencyInjection.Stashbox
{
    public class StashboxDependencyResolver : IDependencyResolver
    {
        private IStashboxContainer Container { get; }

        public StashboxDependencyResolver(IStashboxContainer container)
        {
            Container = container;
        }
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IControllerFactory))
            {
                var stopAndInspect = string.Empty;
            }

            if (typeof(IController).IsAssignableFrom(serviceType))
            {
                var stopAndInspect = string.Empty;
            }

            if (Container.IsRegistered(serviceType))
                return Container.Resolve(serviceType);

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (Container.IsRegistered(serviceType))
                return new List<object> { Container.Resolve(serviceType) };

            return new List<object>();
        }
    }
}