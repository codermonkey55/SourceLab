using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.StructureMap;
using Owin; 

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class StructureMapConfig
    {
        private static StructureMap.IContainer ConfigureContainer()
        {
            var container = StructuremapMvc.Start();

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
