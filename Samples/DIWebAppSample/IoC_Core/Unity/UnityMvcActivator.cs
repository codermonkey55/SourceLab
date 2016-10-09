using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Unity.UnityWebActivator), "PreStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Unity.UnityWebActivator), "Shutdown")]

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Unity
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void PreStart()
        {
            var container = UnityBootstrapper.ConfigurContainer();

            InjectPipline(container);
        }

        /// <summary>Integrates Unity when the application starts.</summary>
        public static IUnityContainer Start()
        {
            var container = UnityBootstrapper.ConfigurContainer();

            InjectPipline(container);

            return container;
        }

        private static void InjectPipline(IUnityContainer container)
        {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());

            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // Uncomment if you want to use PerRequestLifetimeManager
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));

            container.RegisterType(typeof(Controllers.HomeController));

            var factory = new UnityHttpControllerFactory(container);

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), factory);
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityBootstrapper.GetConfiguredContainer();

            container.Dispose();
        }
    }
}