using System;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Owin;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class UnityConfig
    {
        private static IUnityContainer ConfigureContainer()
        {
            var container = UnityWebActivator.Start();

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
