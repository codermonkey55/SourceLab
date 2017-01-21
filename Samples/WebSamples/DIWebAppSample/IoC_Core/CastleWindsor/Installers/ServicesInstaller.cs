using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DIWebAppSample.Services;
using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For<RouteCollection>().UsingFactoryMethod(_ => RouteTable.Routes));
            //container.Register(Component.For<RequestContext>().UsingFactoryMethod(_ => ((MvcHandler)HttpContext.Current.Handler).RequestContext));
            //container.Register(Component.For<UrlHelper>().UsingFactoryMethod(_ => new UrlHelper(_.Resolve<RequestContext>(), _.Resolve<RouteCollection>())));
            //container.Register(Component.For<UrlHelper>().UsingFactoryMethod(_ => new UrlHelper(((MvcHandler)HttpContext.Current.Handler).RequestContext, RouteTable.Routes)));

            //Service Dependencies
            container.Register(Component.For<RouteCollection>().UsingFactoryMethod(_ => RouteTable.Routes));
            container.Register(Component.For<HttpContextBase>().UsingFactoryMethod(_ => new HttpContextWrapper(HttpContext.Current)));
            container.Register(Component.For<RouteData>().UsingFactoryMethod(_ => RouteTable.Routes.GetRouteData(_.Resolve<HttpContextBase>())));
            container.Register(Component.For<MemoryCache>().UsingFactoryMethod(_ => new MemoryCache("CastleWindsorRegistration_Default", new NameValueCollection())));
            container.Register(Component.For<RequestContext>().UsingFactoryMethod(_ => ((MvcHandler)HttpContext.Current.Handler).RequestContext).LifestylePerWebRequest());
            container.Register(Component.For<UrlHelper>().UsingFactoryMethod(_ => new UrlHelper(_.Resolve<RequestContext>(), _.Resolve<RouteCollection>())).LifestylePerWebRequest());

            //Services
            container.Register(Component.For<IFeedService>().ImplementedBy<FeedService>().LifestylePerWebRequest());
            container.Register(Component.For<ICacheService>().ImplementedBy<CacheService>().LifestylePerWebRequest());
            container.Register(Component.For<ILoggingService>().ImplementedBy<LoggingService>().LifestyleSingleton());
            container.Register(Component.For<IRobotsService>().ImplementedBy<RobotsService>().LifestylePerWebRequest());
            container.Register(Component.For<ISitemapService>().ImplementedBy<SitemapService>().LifestylePerWebRequest());
            container.Register(Component.For<IOpenSearchService>().ImplementedBy<OpenSearchService>().LifestylePerWebRequest());
            container.Register(Component.For<ISitemapPingerService>().ImplementedBy<SitemapPingerService>().LifestylePerWebRequest());
        }
    }
}
