using System;
using System.Web.Mvc;
using Stashbox;
using Stashbox.Infrastructure;
using Stashbox.LifeTime;
using WindowsAuthenticationSample.Controllers;
using WindowsAuthenticationSample.DependencyInjection.Stashbox;
using WindowsAuthenticationSample.Models;

//[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WindowsAuthenticationSample.App_Start.StashboxConfig), "Bootstrap")]
//[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(WindowsAuthenticationSample.App_Start.StashboxConfig), "Teardown")]

namespace WindowsAuthenticationSample.App_Start
{
    public class StashboxConfig
    {
        private static IStashboxContainer _container;
        private static IStashboxContainer ConfigureContainer()
        {
            var container = new StashboxContainer();

            RegisterServices(container);

            DependencyResolver.SetResolver(new StashboxDependencyResolver(container));

            return container;
        }

        private static void RegisterServices(IStashboxContainer container)
        {
            container.RegisterType<LinkManager>();

            container.RegisterType<HomeController>();

            container.RegisterType<IAuthorizationManager, AuthorizationManager>();

            container.PrepareType<IProfileConfiguration, ProfileConfiguration>()
                     .WithFactory(() => new ProfileConfiguration())
                     .WithLifetime(new SingletonLifetime())
                     .Register();

            //var userManager = new UserManager<IAuthorizationManager, IProfileConfiguration>();

            //container.BuildUp<UserManager<IAuthorizationManager, IProfileConfiguration>>(userManager);

            container.RegisterType<IUserManager, UserManager<IAuthorizationManager, IProfileConfiguration>>();

            container.PrepareType<IControllerFactory, StashboxControllerFactory>()
                     .WithFactory(() => new StashboxControllerFactory(container))
                     .WithLifetime(new SingletonLifetime())
                     .Register();
        }

        public static void Bootstrap()
        {
            _container = ConfigureContainer();
        }

        public void Teardown()
        {
            try
            {
                _container?.Dispose();
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _container = null;
            }
        }
    }
}