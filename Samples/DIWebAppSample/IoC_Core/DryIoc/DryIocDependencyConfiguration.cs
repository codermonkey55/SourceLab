using CodeLabs.Web.Mvc5.IoC_Integration.IoC_Configs;
using DIWebAppSample.Controllers;
using DIWebAppSample.IoC_Core.Mvc;
using DIWebAppSample.IoC_Core.WebApi;
using DIWebAppSample.Services;
using DryIoc;
using DryIoc.Mvc;
using DryIoc.Web;
using DryIoc.WebApi;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(DryIocConfig), "Start")]
[assembly: ApplicationShutdownMethod(typeof(DryIocConfig), "Shutdown")]

namespace DIWebAppSample.IoC_Core.DryIoc
{
    public class DryIocDependencyConfiguration
    {
        private static IContainer container;

        static DryIocDependencyConfiguration()
        {
            container = new Container();
        }

        public static void Start()
        {
            var configuredContainer = ConfigureContainer();

            configuredContainer.OpenScope();
        }

        public static IContainer ConfigureContainer()
        {
            container.With(rules => rules.With(propertiesAndFields: DeclaredPublicProperties));

            container.SetupMvc(typeof(HomeController).Assembly);
            //-> Or Use the built-in extension which performs the following:
            //----> container = container.With(scopeContext: scopeContext);
            //----> container.RegisterMvcControllers(controllerAssemblies);
            //----> container.SetFilterAttributeFilterProvider(FilterProviders.Providers);
            //----> DependencyResolver.SetResolver(new DryIocDependencyResolver(container));
            container.WithMvc(new[] { typeof(DryIocConfig).Assembly }, new HttpContextScopeContext());

            container.SetupWebApi(typeof(HomeController).Assembly);
            //-> Or Use the built-in extension which performs the following:
            //----> container = container.With(scopeContext: scopeContext ?? new AsyncExecutionFlowScopeContext());
            //----> container.RegisterWebApiControllers(config, controllerAssemblies);
            //----> container.SetFilterProvider(config.Services);
            //----> config.DependencyResolver = new DryIocDependencyResolver(container, throwIfUnresolved);
            container.WithWebApi(GlobalConfiguration.Configuration, new[] { typeof(HomeController).Assembly });

            RegisterDependencies(container);

            return container;
        }

        private static void RegisterDependencies(IContainer container)
        {
            container.Register<IBrowserConfigService, BrowserConfigService>(Reuse.InWebRequest);
            container.Register<ICacheService, CacheService>(Reuse.InWebRequest);
            container.Register<IFeedService, FeedService>(Reuse.InWebRequest);
            container.Register<ILoggingService, LoggingService>(Reuse.InResolutionScope);
            container.Register<IManifestService, ManifestService>(Reuse.Transient);
            container.Register<IOpenSearchService, OpenSearchService>(Reuse.Transient);
            container.Register<ISitemapService, SitemapService>(Reuse.Transient);
            container.Register<ISitemapPingerService, SitemapPingerService>(Reuse.Transient);
            container.RegisterDelegate<IRobotsService>(resolver => new RobotsService(), Reuse.Singleton);
            container.Register<HomeController>(Reuse.InWebRequest);
        }

        private static IEnumerable<PropertyOrFieldServiceInfo> DeclaredPublicProperties(Request request)
        {
            return (request.ImplementationType ?? request.ServiceType).GetTypeInfo()
                .DeclaredProperties.Where(p => p.IsInjectable())
                .Select(PropertyOrFieldServiceInfo.Of);
        }

        public static void Shutdown()
        {
            container.Dispose();
        }
    }
}