using DIWebAppSample.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.SimpleInjector
{
    public class SimpleInjectorServiceLoader
    {
        private Container MvcContainer { get; set; }

        private Container ApiContainer { get; set; }

        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public Container InitializeMvcContainer()
        {
            MvcContainer = new Container();

            MvcContainer.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            MvcContainer.Options.AllowOverridingRegistrations = true;

            MvcContainer.Options.ResolveUnregisteredCollections = true;

            LoadConfigurations(MvcContainer);

            MvcContainer.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            MvcContainer.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(MvcContainer));

            return MvcContainer;
        }

        public Container InitializeApiContainer()
        {

            ApiContainer = new Container();

            ApiContainer.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            ApiContainer.Options.AllowOverridingRegistrations = true;

            ApiContainer.Options.ResolveUnregisteredCollections = true;

            LoadConfigurations(ApiContainer);

            ApiContainer.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            ApiContainer.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(ApiContainer);

            return ApiContainer;
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
            MvcContainer?.Dispose();
            ApiContainer?.Dispose();
        }
    }
}
