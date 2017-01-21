using System;
using Dynamo.Ioc;
using Dynamo.Ioc.Index;
using Dynamo.Ioc.Web;

// Could as-well use the built-in attribute if you don't care about the order
// [assembly: System.Web.PreApplicationStartMethodAttribute(typeof(CodeLabs.Web.Mvc5.IoC_Integration.App_Start.DependencyConfig), "PreStart")]

// When Order is not defined it defaults to -1, so -2 is used to make it run first
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CodeLabs.Web.Mvc5.IoC_Integration.IoC_Configs.DynamoConfig), "PreStart", Order = -2)]

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Dynamo.IoC
{
    public static class DynamoDependencyProvider
    {
        private static IIocContainer _container = default(IIocContainer);

        public static IIocContainer GetIoCContainer()
        {
            try
            {
                if (_container == null)
                    CompileIoCContainer(true);

                return _container;
            }
            finally
            {

                _container = null;
            }
        }

        private static void CompileIoCContainer(bool setContainer)
        {
            var container = new IocContainer(() => new SessionLifetime(), CompileMode.Dynamic, new DirectIndex());

            DynamoRegistration.RegisterDependencies(container);

            var resolver = new DynamoDependencyResolver(container);

            RegisterMvcDependencyResolver(resolver);
            RegisterWebApiDependencyResolver(resolver);
            RegisterModelValidators(resolver);

            if (setContainer) _container = container;
        }



        private static void RegisterMvcDependencyResolver(System.Web.Mvc.IDependencyResolver resolver)
        {
            // Mvc
            System.Web.Mvc.DependencyResolver.SetResolver(resolver);
        }

        private static void RegisterWebApiDependencyResolver(System.Web.Http.Dependencies.IDependencyResolver resolver)
        {
            // Web Api
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static void RegisterModelValidators(IServiceProvider provider)
        {
            // Register Custom Model Validators that enables exposing the Container as IServiceProvider through ValidationContext.GetService() (Validation Attribute etc)
            System.Web.Mvc.DataAnnotationsModelValidatorProvider.RegisterDefaultAdapterFactory((metadata, context, attribute) => new DynamoDataAnnotationsModelValidator(provider, metadata, context, attribute));
            System.Web.Mvc.DataAnnotationsModelValidatorProvider.RegisterDefaultValidatableObjectAdapterFactory((metadata, context) => new DynamoValidatableObjectAdapter(provider, metadata, context));
        }

        public static void PreStart()
        {
            CompileIoCContainer(false);
        }
    }
}