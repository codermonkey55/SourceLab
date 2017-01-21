using SimpleInjector;

[assembly: WebActivator.PostApplicationStartMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector.SimpleInjectorInitializer), "Initialize")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector.SimpleInjectorInitializer), "Abort")]

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector
{
    public static class SimpleInjectorInitializer
    {
        public static SimpleInjectorServiceLoader SimpleInjectorServiceLoader { get; set; }

        /// <summary>
        /// Initialize the container and register it as MVC5 Dependency Resolver.
        /// </summary>
        public static Container Initialize()
        {
            SimpleInjectorServiceLoader = new SimpleInjectorServiceLoader();

            var container = SimpleInjectorServiceLoader.InitializeMvcContainer();

            return container;
        }

        /// <summary>
        /// Initialize the container and register it as WebApi2 Dependency Resolver.
        /// </summary>
        /// <returns></returns>
        public static Container InitializeWebApi()
        {
            SimpleInjectorServiceLoader = new SimpleInjectorServiceLoader();

            var container = SimpleInjectorServiceLoader.InitializeApiContainer();

            return container;
        }

        public static void Abort()
        {
            SimpleInjectorServiceLoader.Dispose();
        }
    }
}