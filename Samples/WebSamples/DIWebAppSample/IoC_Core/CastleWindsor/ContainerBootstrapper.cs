using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Resolver;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor
{
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        public IWindsorContainer Container { get; }

        ContainerBootstrapper(IWindsorContainer container)
        {
            Container = container;
        }

        public static ContainerBootstrapper Bootstrap()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            System.Web.Mvc.DependencyResolver.SetResolver(new WindsorDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new WindsorHttpDependcyResolver(container);

            return new ContainerBootstrapper(container);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}