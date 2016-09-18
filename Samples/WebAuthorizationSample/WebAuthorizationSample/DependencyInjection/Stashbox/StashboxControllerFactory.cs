using System;
using System.Web.Mvc;
using System.Web.Routing;
using Stashbox.Infrastructure;

namespace WindowsAuthenticationSample.DependencyInjection.Stashbox
{
    public class StashboxControllerFactory : DefaultControllerFactory
    {
        private readonly IStashboxContainer _container;

        public StashboxControllerFactory(IStashboxContainer container)
        {
            _container = container;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            string controllername = requestContext.RouteData.Values["controller"].ToString();

            Type controllerType = Type.GetType($"{nameof(WindowsAuthenticationSample)}.{nameof(Controllers)}.{controllername}Controller") ?? GetControllerType(requestContext, controllerName);

            if (controllerType != null)
            {
                var controller = _container.Resolve(controllerType) ?? Activator.CreateInstance(controllerType);

                return controller as IController;
            }

            return null;
        }
        public override void ReleaseController(IController controller)
        {
            IDisposable dispose = controller as IDisposable;

            dispose?.Dispose();
        }
    }
}