﻿/*
The MIT License (MIT)

Copyright (c) 2016 Maksim Volkau

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

namespace DryIoc.Source.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Filters;

    /// <summary>WebApi DI bootstrapper with DryIoc.</summary>
    public static class DryIocWebApi
    {
        /// <summary>Configures container to work with ASP.NET WepAPI by:
        /// setting container scope context to <see cref="AsyncExecutionFlowScopeContext"/> (if scope context is not set already),
        /// registering HTTP controllers, setting filter provider and dependency resolver.</summary>
        /// <param name="container">Original container.</param> <param name="config">Http configuration.</param>
        /// <param name="controllerAssemblies">(optional) Assemblies to look for controllers, default is ExecutingAssembly.</param>
        /// <param name="scopeContext">(optional) Specific scope context to use, by default method sets
        /// <see cref="AsyncExecutionFlowScopeContext"/>, only if container does not have context specified already.</param>
        /// <param name="throwIfUnresolved">(optional) Instructs DryIoc to throw exception
        /// for unresolved type instead of fallback to default Resolver.</param>
        /// <returns>New container.</returns>
        public static IContainer WithWebApi(this IContainer container, HttpConfiguration config,
            IEnumerable<Assembly> controllerAssemblies = null, IScopeContext scopeContext = null,
            Func<Type, bool> throwIfUnresolved = null)
        {
            container.ThrowIfNull();

            if (container.ScopeContext == null)
                container = container.With(scopeContext: scopeContext ?? new AsyncExecutionFlowScopeContext());

            container.RegisterWebApiControllers(config, controllerAssemblies);

            container.SetFilterProvider(config.Services);

            InsertRegisterRequestMessageHandler(config);

            config.DependencyResolver = new DryIocDependencyResolver(container, throwIfUnresolved);

            return container;
        }

        /// <summary>Registers controllers found in provided assemblies with <see cref="Reuse.InWebRequest"/>.</summary>
        /// <param name="container">Container.</param>
        /// <param name="config">Http configuration.</param>
        /// <param name="assemblies">Assemblies to look for controllers.</param>
        public static void RegisterWebApiControllers(this IContainer container, HttpConfiguration config,
            IEnumerable<Assembly> assemblies = null)
        {
            var assembliesResolver = assemblies == null
                 ? config.Services.GetAssembliesResolver()
                 : new GivenAssembliesResolver(assemblies.ToArrayOrSelf());

            var controllerTypeResolver = config.Services.GetHttpControllerTypeResolver();
            var controllerTypes = controllerTypeResolver.GetControllerTypes(assembliesResolver);

            container.RegisterMany(controllerTypes, Reuse.InWebRequest);
        }

        /// <summary>Helps to find if type is controller type.</summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if controller type</returns>
        public static bool IsController(this Type type)
        {
            return ControllerResolver.Default.IsController(type);
        }

        private sealed class ControllerResolver : DefaultHttpControllerTypeResolver
        {
            public static readonly ControllerResolver Default = new ControllerResolver();

            public bool IsController(Type type)
            {
                return IsControllerTypePredicate(type);
            }
        }

        private sealed class GivenAssembliesResolver : IAssembliesResolver
        {
            private readonly ICollection<Assembly> _assemblies;
            public GivenAssembliesResolver(ICollection<Assembly> assemblies) { _assemblies = assemblies; }
            public ICollection<Assembly> GetAssemblies() { return _assemblies; }
        }

        /// <summary>Replaces all filter providers in services with <see cref="DryIocFilterProvider"/>, and registers it in container.</summary>
        /// <param name="container">DryIoc container.</param> <param name="services">Services</param>
        public static void SetFilterProvider(this IContainer container, ServicesContainer services)
        {
            var providers = services.GetFilterProviders();
            services.RemoveAll(typeof(IFilterProvider), _ => true);
            var filterProvider = new DryIocFilterProvider(container, providers);
            services.Add(typeof(IFilterProvider), filterProvider);
            container.RegisterInstance<IFilterProvider>(filterProvider);
        }

        /// <summary>Inserts DryIoc delegating request handler into message handlers.</summary>
        /// <param name="config">Current configuration.</param>
        public static void InsertRegisterRequestMessageHandler(HttpConfiguration config)
        {
            var handlers = config.ThrowIfNull().MessageHandlers;
            if (!handlers.Any(h => h is RegisterRequestMessageHandler))
                handlers.Insert(0, new RegisterRequestMessageHandler());
        }
    }

    /// <summary>Resolve based on DryIoc container.</summary>
    public sealed class DryIocDependencyResolver : IDependencyResolver
    {
        /// <summary>Original DryIoc container.</summary>
        public readonly IContainer Container;

        private readonly Func<Type, bool> _throwIfUnresolved;

        /// <summary>Creates dependency resolver.</summary>
        /// <param name="container">Container.</param>
        /// <param name="throwIfUnresolved">(optional) Instructs DryIoc to throw exception
        /// for unresolved type instead of fallback to default Resolver.</param>
        internal DryIocDependencyResolver(IContainer container, Func<Type, bool> throwIfUnresolved = null)
        {
            Container = container;
            _throwIfUnresolved = throwIfUnresolved;
        }

        /// <summary>Disposes container.</summary>
        public void Dispose()
        {
            if (Container != null)
                Container.Dispose();
        }

        /// <summary>Retrieves a service from the scope or null if unable to resolve service.</summary>
        /// <returns>The retrieved service.</returns> <param name="serviceType">The service to be retrieved.</param>
        public object GetService(Type serviceType)
        {
            var ifUnresolvedReturnDefault = _throwIfUnresolved == null || !_throwIfUnresolved(serviceType);
            return Container.Resolve(serviceType, ifUnresolvedReturnDefault);
        }

        /// <summary>Retrieves a collection of services from the scope or empty collection.</summary>
        /// <returns>The retrieved collection of services.</returns>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.ResolveMany<object>(serviceType);
        }

        /// <summary>Opens scope from underlying container.</summary>
        /// <returns>Opened scope wrapped in dependency scope.</returns>
        public IDependencyScope BeginScope()
        {
            var scope = Container.OpenScope(Reuse.WebRequestScopeName);
            return new DryIocDependencyScope(scope, _throwIfUnresolved);
        }
    }

    /// <summary>Dependency scope adapter to scoped DryIoc container (created by <see cref="IContainer.OpenScope"/>).</summary>
    public sealed class DryIocDependencyScope : IDependencyScope
    {
        /// <summary>Wrapped DryIoc container.</summary>
        public readonly IContainer ScopedContainer;

        private readonly Func<Type, bool> _throwIfUnresolved;

        /// <summary>Adapts input container.</summary>
        /// <param name="scopedContainer">Container returned by OpenScope method.</param>
        /// <param name="throwIfUnresolved">(optional) Instructs DryIoc to throw exception
        /// for unresolved type instead of fallback to default Resolver.</param>
        public DryIocDependencyScope(IContainer scopedContainer, Func<Type, bool> throwIfUnresolved = null)
        {
            ScopedContainer = scopedContainer;
            _throwIfUnresolved = throwIfUnresolved;
        }

        /// <summary>Disposed underlying scoped container.</summary>
        public void Dispose()
        {
            if (ScopedContainer != null)
                ScopedContainer.Dispose();
        }

        /// <summary>Retrieves a service from the scope or returns null if not resolved.</summary>
        /// <returns>The retrieved service.</returns> <param name="serviceType">The service to be retrieved.</param>
        public object GetService(Type serviceType)
        {
            var ifUnresolvedReturnDefault = _throwIfUnresolved == null || !_throwIfUnresolved(serviceType);
            return ScopedContainer.Resolve(serviceType, ifUnresolvedReturnDefault);
        }

        /// <summary>Retrieves a collection of services from the scope or empty collection.</summary>
        /// <returns>The retrieved collection of services.</returns>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ScopedContainer.ResolveMany<object>(serviceType);
        }
    }

    /// <summary>Aggregated filter provider.</summary>
    public sealed class DryIocFilterProvider : IFilterProvider
    {
        /// <summary>Creates filter provider.</summary>
        /// <param name="container"></param> <param name="providers"></param>
        public DryIocFilterProvider(IContainer container, IEnumerable<IFilterProvider> providers)
        {
            _container = container;
            _providers = providers;
        }

        /// <summary> Returns an enumeration of filters. </summary>
        /// <returns> An enumeration of filters. </returns>
        /// <param name="configuration">The HTTP configuration.</param><param name="actionDescriptor">The action descriptor.</param>
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = _providers.SelectMany(p => p.GetFilters(configuration, actionDescriptor)).ToArray();
            for (var i = 0; i < filters.Length; i++)
                _container.InjectPropertiesAndFields(filters[i].Instance);
            return filters;
        }

        private readonly IContainer _container;
        private readonly IEnumerable<IFilterProvider> _providers;
    }

    /// <summary>Registers current <see cref="HttpRequestMessage"/> into dependency scope.</summary>
    internal sealed class RegisterRequestMessageHandler : DelegatingHandler
    {
        /// <summary>Registers request into dependency scope and sends proceed the pipeline.</summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RegisterInDependencyScope(request);
            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>Registers request into current dependency scope.</summary>
        /// <param name="request">Request to register.</param>
        public void RegisterInDependencyScope(HttpRequestMessage request)
        {
            var dependencyScope = request.ThrowIfNull()
                .GetDependencyScope().ThrowIfNotOf(typeof(DryIocDependencyScope), Error.RequestMessageDoesnotReferenceDryiocDependencyScope);

            var container = ((DryIocDependencyScope)dependencyScope).ScopedContainer;
            container.RegisterInstance(request, Reuse.InWebRequest, IfAlreadyRegistered.Replace);
        }
    }

    /// <summary>Possible web exceptions.</summary>
    public static class Error
    {
#pragma warning disable 1591 // "Missing XML-comment"
        public static readonly int
            RequestMessageDoesnotReferenceDryiocDependencyScope = DryIoc.Error.Of(
                "Expecting request message dependency scope to be of type {1} but found: {0}.");
#pragma warning restore 1591
    }
}
