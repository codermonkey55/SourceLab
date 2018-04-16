using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL_WebAPI_SampleNET45.Controllers;
using GraphQL_WebAPI_SampleNET45.Models;
using Stashbox;
using Stashbox.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GraphQL_WebAPI_SampleNET45
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new StashboxContainer();

            container.RegisterType<AdventureType>();
            container.RegisterType<AdventureQuery>();
            container.RegisterSingleton<AdventuresDb>();
            container.RegisterSingleton<IDocumentExecuter, DocumentExecuter>();
            container.RegisterInstanceAs<IDocumentWriter>(new DocumentWriter(true));
            container.RegisterType<ISchema, AdventureSchema>(cfg => cfg.WithSingletonLifetime());
            container.RegisterType<GraphQL.IDependencyResolver>(cfg => cfg.WithFactory(r => new FuncDependencyResolver(type => r.Resolve(type))));
            //...configure container

            container.AddWebApiModelValidatorInjection(config);
            container.AddWebApiFilterProviderInjection(config);
            container.RegisterWebApiControllers(config);

            config.DependencyResolver = new StashboxDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
