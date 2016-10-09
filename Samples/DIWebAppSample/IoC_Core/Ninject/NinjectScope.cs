using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Ninject
{
    /// <summary>
    /// <see cref="http://www.strathweb.com/2012/05/using-ninject-with-the-latest-asp-net-web-api-source/"/>
    /// </summary>
    public class NinjectScope : IDependencyScope
    {
        protected IResolutionRoot ResolutionRoot;

        public NinjectScope(IResolutionRoot kernel)
        {
            ResolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = ResolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return ResolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = ResolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return ResolutionRoot.Resolve(request).ToList();
        }

        public void Dispose()
        {
            IDisposable disposable = (IDisposable)ResolutionRoot;
            disposable?.Dispose();
            ResolutionRoot = null;
        }
    }
}