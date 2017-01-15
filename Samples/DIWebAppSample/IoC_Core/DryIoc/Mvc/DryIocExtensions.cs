using DryIoc;
using DryIoc.Mvc;
using DryIoc.WebApi;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace DIWebAppSample.IoC_Core.Mvc
{
    public static class DryIocExtensions
    {
        public static void SetupMvc(this IContainer container, Assembly assembly)
        {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new DryIocFilterAttributeFilterProvider(container));

            container.SetFilterAttributeFilterProvider(FilterProviders.Providers);
            //-> Or
            container.SetFilterProvider(GlobalConfiguration.Configuration.Services);

            DependencyResolver.SetResolver(new DryIocDependencyResolver(container));

            RegisterMvcControllers(container);
        }

        private static void RegisterMvcControllers(IContainer container)
        {
            var controllerAssemblies = DryIocMvc.GetReferencedAssemblies().Where(a => a.GetTypes().Any(t => t.BaseType == typeof(Controller)));

            container.RegisterMany(controllerAssemblies, type => typeof(IController).IsAssignableFrom(type), Reuse.InWebRequest, FactoryMethod.ConstructorWithResolvableArguments);
        }

        private static void SetFilterAttributeFilterProvider(this IContainer container, Collection<IFilterProvider> filterProviders = null)
        {
            filterProviders = filterProviders ?? FilterProviders.Providers;

            var filterAttributeFilterProviders = filterProviders.OfType<FilterAttributeFilterProvider>().ToArray();
            for (var i = filterAttributeFilterProviders.Length - 1; i >= 0; --i)
            {
                filterProviders.RemoveAt(i);
            }

            var filterProvider = new DryIocFilterAttributeFilterProvider(container);
            filterProviders.Add(filterProvider);

            container.RegisterInstance<IFilterProvider>(filterProvider);
        }
    }
}
