using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Resolver
{
    internal class WindsorHttpDependcyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer _container;

        public WindsorHttpDependcyResolver(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this._container = container;
        }

        public object GetService(Type t)
        {
            return this._container.Kernel.HasComponent(t) ? this._container.Resolve(t) : null;
        }

        public IEnumerable<object> GetServices(Type t)
        {
            return this._container.ResolveAll(t).Cast<object>().ToArray();
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorHttpDependcyResolver(_container.GetChildContainer("HttpScope"));
        }

        public void Dispose()
        {

        }
    }
}
