using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Castle.Windsor;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor;
using Owin;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class WindsorConfig
    {
        private static IWindsorContainer ConfigureContainer()
        {
            var builder = WindsorActivator.Start();

            return builder.Container;
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
