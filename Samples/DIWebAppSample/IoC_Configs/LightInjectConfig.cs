using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.LightInject;
using LightInject;
using Owin;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class LightInjectConfig
    {
        private static IServiceContainer ConfigureContainer()
        {
            var container = LightInjectWebInitializer.Init();

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
