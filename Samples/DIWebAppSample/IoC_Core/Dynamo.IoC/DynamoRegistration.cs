using CodeLabs.Web.WebForms.IoC_Integration.Services;
using Dynamo.Ioc;
using Dynamo.Ioc.Web;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Dynamo.IoC
{
    public class DynamoRegistration
    {
        public static void RegisterDependencies(IIocContainer container)
        {
            // Register your dependencies here.

            container.Register<IBrowserConfigService, BrowserConfigService>().WithRequestLifetime();
            container.Register<ICacheService, CacheService>().WithRequestLifetime();
            container.Register<IFeedService, FeedService>().WithRequestLifetime();
            container.Register<ILoggingService, LoggingService>().WithRequestLifetime();
            container.Register<IManifestService, ManifestService>().WithRequestLifetime();
            container.Register<IOpenSearchService, OpenSearchService>().WithRequestLifetime();
            container.Register<IRobotsService, RobotsService>().WithRequestLifetime();
            container.Register<ISitemapService, SitemapService>().WithRequestLifetime();
            container.Register<ISitemapPingerService, SitemapPingerService>().WithRequestLifetime();
        }
    }
}