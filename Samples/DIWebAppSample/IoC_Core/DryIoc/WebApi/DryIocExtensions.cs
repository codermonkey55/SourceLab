using DIWebAppSample.IoC_Core.Extensions;
using DryIoc;
using System.Reflection;
using System.Web.Http;

namespace DIWebAppSample.IoC_Core.WebApi
{
    public static class DryIocExtensions
    {
        public static void SetupWebApi(this IContainer container, Assembly assembly)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new DryIocDependencyResolver(container);
            container.RegisterInstancesOf<ApiController>(assembly);
        }
    }
}
