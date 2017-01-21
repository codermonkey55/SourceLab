using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector;
using Owin;
using SimpleInjector;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class SimpleInjectorConfig
    {
        private static Container ConfigureContainer()
        {
            var container = SimpleInjectorInitializer.Initialize();

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
