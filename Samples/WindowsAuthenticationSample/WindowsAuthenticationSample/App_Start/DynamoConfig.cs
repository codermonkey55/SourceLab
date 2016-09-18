using System;
using Dynamo.Ioc;
using Dynamo.Ioc.Index;
using Dynamo.Ioc.Web;
using WindowsAuthenticationSample.Controllers;
using WindowsAuthenticationSample.Models;

// Could as-well use the built-in attribute if you don't care about the order
// [assembly: System.Web.PreApplicationStartMethodAttribute(typeof(WindowsAuthenticationSample.App_Start.DependencyConfig), "PreStart")]

// When Order is not defined it defaults to -1, so -2 is used to make it run first
//[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WindowsAuthenticationSample.App_Start.DynamoConfig), "PreStart", Order = -2)]

namespace WindowsAuthenticationSample.App_Start
{
    internal static class DynamoConfig
    {
        public static void RegisterDependencies(IIocContainer container)
        {
            // Register your dependencies here.
            //container.RegisterLambda(_ => _.Resolve<HomeController>());
            container.Register<HomeController, HomeController>().WithRequestLifetime();
            container.Register<IAuthorizationManager, AuthorizationManager>().WithRequestLifetime();
            container.Register<IProfileConfiguration, ProfileConfiguration>().WithSessionLifetime();
            container.Register<IUserManager, UserManager<IAuthorizationManager, IProfileConfiguration>>().WithRequestLifetime();
        }

        public static void RegisterMvcDependencyResolver(System.Web.Mvc.IDependencyResolver resolver)
        {
            // Mvc
            System.Web.Mvc.DependencyResolver.SetResolver(resolver);
        }

        public static void RegisterWebApiDependencyResolver(System.Web.Http.Dependencies.IDependencyResolver resolver)
        {
            // Web Api,
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        public static void RegisterModelValidators(IServiceProvider provider)
        {
            // Register Custom Model Validators that enables exposing the Container as IServiceProvider through ValidationContext.GetService() (Validation Attribute etc)
            System.Web.Mvc.DataAnnotationsModelValidatorProvider.RegisterDefaultAdapterFactory((metadata, context, attribute) => new DynamoDataAnnotationsModelValidator(provider, metadata, context, attribute));
            System.Web.Mvc.DataAnnotationsModelValidatorProvider.RegisterDefaultValidatableObjectAdapterFactory((metadata, context) => new DynamoValidatableObjectAdapter(provider, metadata, context));
        }

        public static void PreStart()
        {
            var container = new IocContainer(() => new SessionLifetime(), CompileMode.Dynamic, new DirectIndex());

            RegisterDependencies(container);

            var resolver = new DynamoDependencyResolver(container);

            RegisterMvcDependencyResolver(resolver);
            RegisterWebApiDependencyResolver(resolver);
            RegisterModelValidators(resolver);
        }
    }
}
