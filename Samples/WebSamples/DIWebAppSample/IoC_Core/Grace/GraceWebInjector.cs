using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Grace.DependencyInjection;
using Grace.MVC.Extensions;

[assembly: WebActivator.PostApplicationStartMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Grace.GraceWebInjector), "Begin")]
[assembly: WebActivator.ApplicationShutdownMethod(typeof(CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Grace.GraceWebInjector), "Quit")]

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Grace
{
    public class GraceWebInjector
    {
        internal static IDependencyInjectionContainer DependencyInjectionContainer { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IDependencyInjectionContainer Initiate()
        {
            DependencyInjectionContainer = new DependencyInjectionContainer { ThrowExceptions = true };

            Assembly assembly = Assembly.GetExecutingAssembly();

            GraceDependencyCatalog.RegisterServices(DependencyInjectionContainer);

            RegisterMvc(DependencyInjectionContainer, assembly);

            var registrations = DependencyInjectionContainer.WhatDoIHave();

            SetMvcDependencyResolver(DependencyInjectionContainer);

            return DependencyInjectionContainer;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="diContainer"></param>
        /// <param name="assembly"></param>
        private static void RegisterMvc(IDependencyInjectionContainer diContainer, Assembly assembly)
        {
            // Register Common MVC Types
            diContainer.Configure(c =>
            {
                c.Export<MemoryCache>()
                .WithCtorParam((scope, context) => "CastleWindsorRegistration_Default")
                .WithCtorParam((scope, context) => new NameValueCollection());

                c.Export<RequestContext>()
                .WithCtorParam((scope, context) => new HttpContextWrapper(HttpContext.Current))
                .WithCtorParam((scope, context) => RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current))).Lifestyle.SingletonPerRequest();

                c.Export<UrlHelper>()
                .WithCtorParam((scope, context) => context.Locate<RequestContext>())
                .WithCtorParam((scope, context) => RouteTable.Routes).Lifestyle.SingletonPerRequest();
            });

            // Register MVC Filters


            // Register MVC Controllers
            diContainer.Configure(c =>
            {
                c.ExportAssembly(assembly).ByInterface<IController>();
            });
        }

        /// <summary>
        /// Sets the ASP.NET MVC dependency resolver.
        /// </summary>
        /// <param name="diContainer">The container.</param>
        private static void SetMvcDependencyResolver(IDependencyInjectionContainer diContainer)
        {
            DependencyResolver.SetResolver(new GraceDependencyResolver(diContainer));
        }

        public static void Quit()
        {
            DependencyInjectionContainer.Dispose();
        }
    }
}
