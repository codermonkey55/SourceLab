using System;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Practices.Unity;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Unity
{
    /// <summary>
    /// <see cref="http://dotnet-experience.blogspot.com/2012/06/update-unity-with-aspnet-web-api-4-rc.html"/>
    /// </summary>
    public class UnityHttpControllerFactory : IHttpControllerActivator
    {
        private readonly IUnityContainer _container;
        private readonly DefaultHttpControllerActivator _defaultActivator;

        public UnityHttpControllerFactory(IUnityContainer container)
        {
            this._container = container;
            _defaultActivator = new DefaultHttpControllerActivator();
        }

        public IHttpController Create(
            System.Net.Http.HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            if (_container.IsRegistered(controllerType))
            {
                return _container.Resolve(controllerType) as IHttpController;
            }

            return _defaultActivator.Create(request, controllerDescriptor, controllerType);
        }
    }
}