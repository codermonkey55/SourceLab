using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LightInject;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.LightInject.LightInjectWebInitializer), "Init")]
[assembly: ApplicationShutdownMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.LightInject.LightInjectWebInitializer), "Terminate")]

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.LightInject
{
    public class LightInjectWebInitializer
    {
        public static LightInjectServiceRegistry ServiceRegistry { get; set; }

        /// <summary>
        /// Initialize the container and register it as MVC Dependency Resolver.
        /// </summary>
        public static IServiceContainer Init()
        {
            ServiceRegistry = new LightInjectServiceRegistry();

            var container = ServiceRegistry.InitializeContainer();

            //Registers all the Mvc Controllers implementations found in this assembly.
            container.RegisterControllers();

            // Performs the followings tasks:
            // --> Enables Per Web Request Scope
            // --> Sets the Dependency Resolver
            // --> Initializes the Filter Attribute Provider
            container.EnableMvc();

            return container;
        }

        public static void Terminate()
        {
            ServiceRegistry.Dispose();
        }
    }
}
