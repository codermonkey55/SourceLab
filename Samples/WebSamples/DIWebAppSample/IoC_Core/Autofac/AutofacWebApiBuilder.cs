using Autofac;
using Autofac.Integration.WebApi;
using DIWebAppSample;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Autofac
{
    public class AutofacWebApiBuilder
    {
        internal static IContainer BuildRegistrations()
        {
            var builder = new ContainerBuilder();

            // Register our custom dependencies
            builder.RegisterModule(new ServicesModule());

            // Register dependencies in webapi controllers
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // Register autofac's provider for webapi's modelbinders.
            builder.RegisterWebApiModelBinderProvider();

            //Obsolete
            //builder.RegisterWebApiModelBinders(typeof(MvcApplication).Assembly);

            builder.RegisterType<WebApiRequestContextModelBinder>().AsModelBinderForTypes(typeof(RequestContext));

            var container = builder.Build();

            // Set WebApi DI resolver to use our Autofac container
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }

    [ModelBinder(typeof(RequestContext))]
    internal class WebApiRequestContextModelBinder
    {

    }
}
