using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.CastleWindsor.Plumbing;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Plumbing;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Installers
{
    public class WebTypesInstaller : IWindsorInstaller
    {

        /// <summary>
        /// <see cref="http://blog.nikosbaxevanis.com/2012/03/16/using-the-web-api-dependency-resolver-with-castle-windsor/"/>
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorHttpControllerFactory>()
                                        .ImplementedBy<WindsorHttpControllerFactory>()
                                        .LifestyleSingleton(),

                               //Component.For<IControllerFactory>()
                               //         .ImplementedBy<DefaultControllerFactory>()
                               //         .LifestyleTransient(),

                               Component.For<IControllerFactory>()
                                        .ImplementedBy<WindsorControllerFactory>()
                                        .LifestyleSingleton(),

                               Component.For<ILogger>()
                                        .ImplementedBy<NullLogger>()
                                        .LifestyleSingleton(),

                               //Component.For<IHttpControllerActivator>()
                               //         .ImplementedBy<DefaultHttpControllerActivator>()
                               //         .LifestyleTransient(),

                               Component.For<IHttpControllerActivator>()
                                        .ImplementedBy<WindsorHttpActivator>()
                                        .LifestyleSingleton(),

                               Component.For<IHttpActionSelector>()
                                        .ImplementedBy<ApiControllerActionSelector>()
                                        .LifestyleTransient(),

                               Component.For<IActionValueBinder>()
                                        .ImplementedBy<DefaultActionValueBinder>()
                                        .LifestyleTransient(),

                               Component.For<IHttpActionInvoker>()
                                        .ImplementedBy<ApiControllerActionInvoker>()
                                        .LifestyleTransient(),

                               Component.For<ModelMetadataProvider>()
                                        .ImplementedBy<CachedDataAnnotationsModelMetadataProvider>()
                                        .LifestyleTransient(),

                               Component.For<HttpConfiguration>()
                                        .Instance(GlobalConfiguration.Configuration));
        }
    }
}
