using System.Web.Http.Dependencies;
using Ninject;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Ninject
{
    /// <summary>
    /// <see cref="http://www.strathweb.com/2012/05/using-ninject-with-the-latest-asp-net-web-api-source/"/>
    /// </summary>
    public class NinjectHttpResolver : NinjectScope, IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectHttpResolver(IKernel kernel)
        : base(kernel)
        {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectScope(_kernel.BeginBlock());
        }
    }
}