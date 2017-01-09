using Castle.Windsor;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Plumbing
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        readonly IWindsorContainer _container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// <see cref="http://stackoverflow.com/questions/21929557/find-if-request-is-child-action-request-before-controller-context-is-available"/>
        /// <see cref="http://stackoverflow.com/questions/5425920/asp-net-mvc-controller-created-for-every-request"/>
        /// Asp.NET Mvc calls to get controller for each request.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var handler = requestContext.HttpContext.CurrentHandler;

            var isChildAction1 = ((MvcHandler)handler).RequestContext.RouteData.DataTokens.ContainsKey("ParentActionViewContext");
            //-> Or
            var isChildAction2 = isChildAction1 || requestContext.HttpContext.PreviousHandler != null && requestContext.HttpContext.PreviousHandler is MvcHandler;

            if (controllerType != null && _container.Kernel.HasComponent(controllerType))
                return (IController)_container.Resolve(controllerType);

            return base.GetControllerInstance(requestContext, controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
        }
    }
}