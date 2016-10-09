using CodeLabs.Web.WebForms.IoC_Integration.Services;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject.Modules
{

    internal class ServiceModule : NinjectModule
    {
        /// <summary>
        /// Register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public override void Load()
        {
            Bind<ICacheService>().To<CacheService>().InRequestScope();
            Bind<IFeedService>().To<FeedService>().InRequestScope();
            Bind<ILoggingService>().To<LoggingService>().InSingletonScope();
            Bind<IOpenSearchService>().To<OpenSearchService>().InRequestScope();
            Bind<IRobotsService>().To<RobotsService>().InRequestScope();
            Bind<ISitemapService>().To<SitemapService>().InRequestScope();
            Bind<ISitemapPingerService>().To<SitemapPingerService>().InRequestScope();
        }
    }

}