using System;
using System.Linq;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CodeLabs.Web.WebForms.IoC_Integration.Controllers;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.Installers
{
    public class HttpControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (Type controller in typeof(HomeController).Assembly.GetTypes()
                .Where(type => typeof(IHttpController).IsAssignableFrom(type)))
            {
                // https://github.com/srkirkland/Inflector/
                string name = Inflector.Inflector.Pluralize(
                    controller.Name.Replace("Controller", ""));

                container.Register(Component
                    .For(controller)
                    .Named(name)
                    .LifestylePerWebRequest());
            }
        }
    }
}