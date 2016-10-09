using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Ninject.Factories
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public NinjectControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        // asp.net mvc calls to get controller for each request
        protected override IController GetControllerInstance(RequestContext ctx, Type ctlrType)
        {
            if (ctlrType == null)
                return null;

            return _kernel.Get(ctlrType) as IController;
        }
    }
}