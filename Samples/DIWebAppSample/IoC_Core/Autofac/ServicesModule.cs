using Autofac;
using DIWebAppSample.Services;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Autofac
{
    public class ServicesModule : Module
    {
        public ServicesModule()
        {

        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BrowserConfigService>().As<IBrowserConfigService>().InstancePerRequest();
            builder.RegisterType<CacheService>().As<ICacheService>().InstancePerRequest();
            builder.RegisterType<FeedService>().As<IFeedService>().InstancePerRequest();
            builder.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();
            builder.RegisterType<ManifestService>().As<IManifestService>().InstancePerRequest();
            builder.RegisterType<OpenSearchService>().As<IOpenSearchService>().InstancePerRequest();
            builder.RegisterType<RobotsService>().As<IRobotsService>().InstancePerRequest();
            builder.RegisterType<SitemapService>().As<ISitemapService>().InstancePerRequest();
            builder.RegisterType<SitemapPingerService>().As<ISitemapPingerService>().InstancePerRequest();

            base.Load(builder);
        }
    }
}
