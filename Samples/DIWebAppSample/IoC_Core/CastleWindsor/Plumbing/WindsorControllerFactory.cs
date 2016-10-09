using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Plumbing
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        readonly IWindsorContainer _container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            this._container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
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