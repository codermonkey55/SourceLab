using System.Reflection;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector.SimpleInjectorInitializer), "Initialize")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector.SimpleInjectorInitializer), "Abort")]

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector
{
    public static class SimpleInjectorInitializer
    {
        public static SimpleInjectorServiceLoader SimpleInjectorServiceLoader { get; set; }

        /// <summary>
        /// Initialize the container and register it as MVC3 Dependency Resolver.
        /// </summary>
        public static Container Initialize()
        {
            SimpleInjectorServiceLoader = new SimpleInjectorServiceLoader();

            var container = SimpleInjectorServiceLoader.InitializeContainer();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public static void Abort()
        {
            SimpleInjectorServiceLoader.Dispose();
        }
    }
}