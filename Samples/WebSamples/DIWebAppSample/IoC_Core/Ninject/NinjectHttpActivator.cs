using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Ninject;

namespace CodeLabs.Web.Mvc5.IoC_Integration.IoC_Core.Ninject
{
    /// <summary>
    /// <see cref="https://gist.github.com/paigecook/3860942"/>
    /// </summary>
    public class NinjectHttpActivator : IHttpControllerActivator
    {
        private readonly IKernel _kernel;

        public NinjectHttpActivator(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = (IHttpController)_kernel.Get(controllerType);

            request.RegisterForDispose(new Release(() => _kernel.Release(controller)));

            return controller;
        }
    }

    internal class Release : IDisposable
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