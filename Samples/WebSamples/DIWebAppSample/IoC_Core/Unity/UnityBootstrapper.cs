using DIWebAppSample.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Unity
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityBootstrapper
    {
        #region Unity Container

        private static readonly Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        });

        private static IUnityContainer _configuredContainer;

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer ConfigurContainer()
        {
            return (_configuredContainer = _container.Value);
        }

        public static IUnityContainer GetConfiguredContainer()
        {
            return _configuredContainer;
        }

        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<RouteCollection>(new InjectionFactory(_ => RouteTable.Routes));
            container.RegisterType<HttpContextBase>(new InjectionFactory(_ => new HttpContextWrapper(HttpContext.Current)));
            container.RegisterType<RequestContext>(new InjectionFactory(_ => ((MvcHandler)HttpContext.Current.Handler).RequestContext));
            container.RegisterType<RouteData>(new InjectionFactory(_ => RouteTable.Routes.GetRouteData(container.Resolve<HttpContextBase>())));
            container.RegisterType<MemoryCache>(new InjectionFactory(_ => new MemoryCache("UnityRegistration_Default", new NameValueCollection())));

            container.RegisterType<IBrowserConfigService, BrowserConfigService>(new PerRequestLifetimeManager());
            container.RegisterType<IOpenSearchService, OpenSearchService>(new PerRequestLifetimeManager());
            container.RegisterType<IManifestService, ManifestService>(new PerRequestLifetimeManager());
            container.RegisterType<IRobotsService, RobotsService>(new PerRequestLifetimeManager());
            container.RegisterType<IFeedService, FeedService>(new PerRequestLifetimeManager());

            container.RegisterType<ICacheService, CacheService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISitemapService, SitemapService>(new PerRequestLifetimeManager());
            container.RegisterType<ILoggingService, LoggingService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISitemapPingerService, SitemapPingerService>(new PerRequestLifetimeManager());

        }
    }
}
