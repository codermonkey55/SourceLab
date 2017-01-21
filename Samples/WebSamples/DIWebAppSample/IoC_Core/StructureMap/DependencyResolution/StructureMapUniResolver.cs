using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.StructureMap.DependencyResolution;
using StructureMap;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.StructureMap.DependencyResolution
{
    /// <summary>
    /// <see cref="http://benfoster.io/blog/per-request-dependencies-in-aspnet-web-api-using-structuremap"/>
    /// </summary>
    public class StructureMapUniResolver :
        StructureMapDependencyScope, IDependencyResolver, IHttpControllerActivator
    {
        private readonly IContainer _container;

        public StructureMapUniResolver(IContainer container)
            : base(container)
        {
            this._container = container;
            container.Inject<IHttpControllerActivator>(this);
        }

        public IDependencyScope BeginScope()
        {
            CreateNestedContainer();

            return new StructureMapUniResolver(CurrentNestedContainer);
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var scope = request.GetDependencyScope();
            return scope.GetService(controllerType) as IHttpController;
        }
    }
}