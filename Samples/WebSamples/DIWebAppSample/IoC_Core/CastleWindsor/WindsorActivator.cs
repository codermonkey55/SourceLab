using System;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.WindsorActivator), "PreStart")]
[assembly: ApplicationShutdownMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor.WindsorActivator), "Shutdown")]

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.CastleWindsor
{
    public static class WindsorActivator
    {
        static ContainerBootstrapper _bootstrapper;

        public static void PreStart()
        {
            _bootstrapper = ContainerBootstrapper.Bootstrap();
        }

        public static ContainerBootstrapper Start()
        {
            _bootstrapper = ContainerBootstrapper.Bootstrap();

            return _bootstrapper;
        }

        public static void Shutdown()
        {
            if (_bootstrapper == null) return;

            _bootstrapper.Dispose();
        }
    }
}