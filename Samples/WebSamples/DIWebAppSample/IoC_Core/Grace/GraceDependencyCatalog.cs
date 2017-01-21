using DIWebAppSample.Services;
using Grace.DependencyInjection;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Grace
{
    public class GraceDependencyCatalog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="diContainer"></param>
        internal static void RegisterServices(IDependencyInjectionContainer diContainer)
        {
            diContainer.Configure(c =>
            {
                c.Export<CacheService>().As<ICacheService>();
                c.Export<FeedService>().As<IFeedService>();
                c.Export<LoggingService>().As<ILoggingService>().Lifestyle.Singleton();
                c.Export<OpenSearchService>().As<IOpenSearchService>();
                c.Export<RobotsService>().As<IRobotsService>();
                c.Export<SitemapService>().As<ISitemapService>();
                c.Export<SitemapPingerService>().As<ISitemapPingerService>();
            });
        }
    }
}
