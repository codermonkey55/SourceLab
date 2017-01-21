using System.Web.Http.Controllers;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.CastleWindsor.Plumbing
{
    public interface IWindsorHttpControllerFactory
    {
        IHttpController CreateController(HttpControllerContext controllerContext, string controllerName);
        void ReleaseController(IHttpController controller);
    }
}