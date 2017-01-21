using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.CastleWindsor.Plumbing
{
    /// <summary>
    ///  <see cref="http://www.codeproject.com/Articles/350488/A-simple-POC-using-ASP-NET-Web-API-Entity-Framewor"/>
    /// </summary>
    public class WindsorHttpActivator : IHttpControllerActivator
    {
        private readonly IWindsorContainer _container;

        public WindsorHttpActivator(IWindsorContainer container)
        {
            _container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller =
                (IHttpController)_container.Resolve(controllerType);

            request.RegisterForDispose(
                new Release(
                    () => _container.Release(controller)));

            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action _release;

            public Release(Action release)
            {
                _release = release;
            }

            public void Dispose()
            {
                _release();
            }
        }

    }
}