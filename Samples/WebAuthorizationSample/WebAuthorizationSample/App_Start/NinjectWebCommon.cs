using System.Web.Mvc;
using Ninject.Web.Mvc;
using WindowsAuthenticationSample.Controllers;
using WindowsAuthenticationSample.Models;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WindowsAuthenticationSample.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WindowsAuthenticationSample.App_Start.NinjectWebCommon), "Stop")]

namespace WindowsAuthenticationSample.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            Bootstrapper.Initialize(CreateKernel);

            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<HomeController>().ToSelf().InRequestScope();
            kernel.Bind<IAuthorizationManager>().To<AuthorizationManager>().InRequestScope();
            kernel.Bind<IProfileConfiguration>().To<ProfileConfiguration>().InSingletonScope();
            kernel.Bind<IUserManager>().To<UserManager<IAuthorizationManager, IProfileConfiguration>>().InRequestScope();
        }
    }
}
