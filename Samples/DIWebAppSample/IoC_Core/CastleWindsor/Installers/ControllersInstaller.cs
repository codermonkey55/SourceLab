using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Plumbing;
using System.Web.Mvc;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.
                    FromThisAssembly().
                    BasedOn<IController>().
                    If(c => c.Name.EndsWith("Controller"))
                    .LifestylePerWebRequest());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
            ControllerBuilder.Current.DefaultNamespaces.Add("DIWebAppSample.Controllers");
        }
    }
}