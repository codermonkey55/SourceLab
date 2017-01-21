using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Ninject.Factories
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public NinjectControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// <see cref="http://stackoverflow.com/questions/21929557/find-if-request-is-child-action-request-before-controller-context-is-available"/>
        /// <see cref="http://stackoverflow.com/questions/5425920/asp-net-mvc-controller-created-for-every-request"/>
        /// Asp.NET Mvc calls to get controller for each request.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ctlrType"></param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext ctx, Type ctlrType)
        {
            var handler = ctx.HttpContext.CurrentHandler;

            var isChildAction1 = ((MvcHandler)handler).RequestContext.RouteData.DataTokens.ContainsKey("ParentActionViewContext");
            //-> Or
            var isChildAction2 = isChildAction1 || ctx.HttpContext.PreviousHandler != null && ctx.HttpContext.PreviousHandler is MvcHandler;

            if (ctlrType == null)
                return null;

            return _kernel.Get(ctlrType) as IController;
        }
    }
}