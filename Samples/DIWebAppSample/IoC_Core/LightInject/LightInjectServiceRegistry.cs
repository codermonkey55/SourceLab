using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeLabs.Web.WebForms.IoC_Integration.Services;
using LightInject;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.LightInject
{
    public class LightInjectServiceRegistry
    {
        internal IServiceContainer Container { get; set; }

        public IServiceContainer InitializeContainer()
        {
            Container = new ServiceContainer();

            LoadConfigurations(Container);

            return Container;
        }

        private void LoadConfigurations(IServiceContainer container)
        {
            // For instance:                                
            container.Register<IBrowserConfigService, BrowserConfigService>(new PerScopeLifetime());
            container.Register<ICacheService, CacheService>(new PerScopeLifetime());
            container.Register<IFeedService, FeedService>(new PerScopeLifetime());
            container.Register<ILoggingService, LoggingService>(new PerContainerLifetime());
            container.Register<IManifestService, ManifestService>(new PerScopeLifetime());
            container.Register<IOpenSearchService, OpenSearchService>(new PerScopeLifetime());
            container.Register<IRobotsService, RobotsService>(new PerScopeLifetime());
            container.Register<ISitemapService, SitemapService>(new PerScopeLifetime());
            container.Register<ISitemapPingerService, SitemapPingerService>(new PerScopeLifetime());
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
