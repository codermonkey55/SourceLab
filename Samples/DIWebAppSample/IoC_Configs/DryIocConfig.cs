using CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Dynamo.IoC;
using Owin;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Configs
{
    internal static class DryIocConfig
    {
        private static Dynamo.Ioc.IIocContainer ConfigureContainer()
        {
            var container = DynamoDependencyProvider.GetIoCContainer();

            return container;
        }

        public static void Bootstrap()
        {
            var container = ConfigureContainer();
        }

        public static void Bootstrap(IAppBuilder app)
        {
            var container = ConfigureContainer();
        }
    }
}
