using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject;
using Owin;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class NinjectConfig
    {
        private static Ninject.IKernel ConfigureContainer()
        {
            var container = NinjectWebCommon.Start();

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
