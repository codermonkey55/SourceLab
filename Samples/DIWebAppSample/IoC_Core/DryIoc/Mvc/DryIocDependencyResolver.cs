using DryIoc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DIWebAppSample.IoC_Core.Mvc
{
    public class DryIocDependencyResolver : IDependencyResolver
    {
        private readonly IContainer container;

        public DryIocDependencyResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
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
    }
}