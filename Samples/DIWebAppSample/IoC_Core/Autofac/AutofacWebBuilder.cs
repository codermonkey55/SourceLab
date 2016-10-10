using Autofac;
using Autofac.Integration.Mvc;
using DIWebAppSample;
using System.Web.Mvc;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Autofac
{
    public class AutofacWebBuilder
    {
        internal static IContainer BuildRegistrations()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register our custom dependencies
            builder.RegisterModule(new ServicesModule("MVCWithAutofacServices"));

            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        }
    }
}
