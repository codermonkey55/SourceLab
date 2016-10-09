using System.Web.Http;
using System.Web.Mvc;
using CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Ninject;
using CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Ninject.Factories;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject.NinjectWebCommon), "PreStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject.NinjectWebCommon), "Stop")]

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void PreStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));

            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));

            Bootstrapper.Initialize(NinjectBootstrapper.CreateKernel(true));

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(Bootstrapper.Kernel));

            DependencyResolver.SetResolver(new NinjectDependencyResolver(Bootstrapper.Kernel));

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectHttpResolver(Bootstrapper.Kernel);
        }

        /// <summary>
        /// Starts the application
        /// </summary>
        public static IKernel Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));

            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));

            Bootstrapper.Initialize(NinjectBootstrapper.CreateKernel(true));

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(Bootstrapper.Kernel));

            DependencyResolver.SetResolver(new NinjectDependencyResolver(Bootstrapper.Kernel));

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectHttpResolver(Bootstrapper.Kernel);

            return Bootstrapper.Kernel;
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
    }
}
