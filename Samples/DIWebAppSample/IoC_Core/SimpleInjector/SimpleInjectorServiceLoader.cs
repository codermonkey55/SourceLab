using DIWebAppSample.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using System.Reflection;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector
{
    public class SimpleInjectorServiceLoader
    {
        private Container Container { get; set; }

        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public Container InitializeContainer()
        {
            Container = new Container();

            Container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            Container.Options.AllowOverridingRegistrations = true;

            Container.Options.ResolveUnregisteredCollections = true;

            LoadConfigurations(Container);

            Container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            Container.Verify();

            return Container;
        }

        private void LoadConfigurations(Container container)
        {
            // For instance:
            container.Register<IBrowserConfigService, BrowserConfigService>(Lifestyle.Transient);
            container.Register<ICacheService, CacheService>(Lifestyle.Transient);
            container.Register<IFeedService, FeedService>(Lifestyle.Transient);
            container.Register<ILoggingService, LoggingService>(Lifestyle.Scoped);
            container.Register<IManifestService, ManifestService>(Lifestyle.Transient);
            container.Register<IOpenSearchService, OpenSearchService>(Lifestyle.Transient);
            container.Register<IRobotsService, RobotsService>(Lifestyle.Transient);
            container.Register<ISitemapService, SitemapService>(Lifestyle.Transient);
            container.Register<ISitemapPingerService, SitemapPingerService>(Lifestyle.Transient);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
