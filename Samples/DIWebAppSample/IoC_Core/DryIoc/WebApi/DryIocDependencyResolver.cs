using DryIoc;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace DIWebAppSample.IoC_Core.WebApi
{
    public class DryIocDependencyResolver : IDependencyResolver
    {
        private IContainer container;
        private SharedDependencyScope sharedScope;

        public DryIocDependencyResolver(IContainer container)
        {
            this.container = container;
            this.sharedScope = new SharedDependencyScope(container);
        }

        public IDependencyScope BeginScope()
        {
            return this.sharedScope;
        }

        public void Dispose()
        {
            this.container.Dispose();
            this.sharedScope.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return this.container.Resolve(serviceType);
            }
            catch (ContainerException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this.container.ResolveMany(serviceType);
            }
            catch (ContainerException)
            {
                return null;
            }
        }

        private sealed class SharedDependencyScope : IDependencyScope
        {
            private IResolver container;

            public SharedDependencyScope(IResolver container)
            {
                this.container = container;
            }

            public object GetService(Type serviceType)
            {
                return this.container.Resolve(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return this.container.ResolveMany(serviceType);
            }

            public void Dispose()
            {
                // NO-OP, as the container is shared.
            }
        }
    }
}