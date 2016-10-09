using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.MicroKernel;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.CastleWindsor.Plumbing
{
    public class WindsorHttpControllerFactory : IWindsorHttpControllerFactory
    {
        private readonly HttpConfiguration _configuration;
        private readonly IKernel _kernel;

        public WindsorHttpControllerFactory(
            HttpConfiguration configuration,
            IKernel kernel)
        {
            this._configuration = configuration;
            this._kernel = kernel;
        }

        public IHttpController CreateController(
            HttpControllerContext controllerContext,
            string controllerName)
        {
            var controller = this._kernel.Resolve<IHttpController>(controllerName);

            controllerContext.Controller = controller;
            controllerContext.ControllerDescriptor = new HttpControllerDescriptor(
                this._configuration,
                controllerName,
                controller.GetType());

            return controllerContext.Controller;
        }

        public void ReleaseController(IHttpController controller)
        {
            this._kernel.ReleaseComponent(controller);
        }
    }
}