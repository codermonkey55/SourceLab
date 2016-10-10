using DIWebAppSample.IoC_Core.DryIoc;
using DryIoc;
using Owin;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Configs
{
    internal static class DryIocConfig
    {
        private static IContainer ConfigureContainer()
        {
            var container = DryIocDependencyConfiguration.ConfigureContainer();

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
