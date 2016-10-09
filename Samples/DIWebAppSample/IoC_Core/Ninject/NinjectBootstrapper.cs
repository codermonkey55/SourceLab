using System;
using System.Reflection;
using System.Web;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject.Modules;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject
{
    public class NinjectBootstrapper
    {
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        internal static Func<IKernel> CreateKernel(bool useStaticInit)
        {
            return () =>
            {
                IKernel kernel;

                if (useStaticInit)
                    kernel = InitWithModules();
                else
                {
                    kernel = new StandardKernel();
                    LoadModules(kernel);
                }

                try
                {
                    kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);

                    kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                    return kernel;
                }
                catch
                {
                    kernel.Dispose();

                    throw;
                }
            };
        }

        /// <summary>
        /// Create kernel with static modules.
        /// </summary>
        private static IKernel InitWithModules()
        {
            INinjectModule[] modules = new INinjectModule[]
            {
                new AutoControllerModule(Assembly.GetExecutingAssembly()),
                new ServiceModule()
            };

            var kernel = new StandardKernel();

            return kernel;
        }

        /// <summary>
        /// Load your modules here
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void LoadModules(IKernel kernel)
        {
            kernel.Load(AppDomain.CurrentDomain.GetAssemblies());
        }
    }

}
