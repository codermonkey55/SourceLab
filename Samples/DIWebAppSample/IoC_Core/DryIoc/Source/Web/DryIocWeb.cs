﻿[assembly: System.Web.PreApplicationStartMethod(typeof(DryIoc.Web.DryIocHttpModuleInitializer), "Initialize")]

namespace DryIoc.Source.Web
{
    using System;
    using System.Collections;
    using System.Threading;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    /// <summary>Extension to get container with ambient <see cref="HttpContext.Current"/> scope context.</summary>
    public static class DryIocWeb
    {
        /// <summary>Creates new container from original with HttpContext or arbitrary/test context <paramref name="getContextItems"/>.</summary>
        /// <param name="container">Original container with some rules and registrations.</param>
        /// <param name="getContextItems">(optional) Arbitrary or test context to use instead of <see cref="HttpContext.Current"/>.</param>
        /// <returns>New container with the same rules and registrations/cache but with new ambient context.</returns>
        public static IContainer WithHttpContextScopeContext(this IContainer container, Func<IDictionary> getContextItems = null)
        {
            return container.ThrowIfNull().With(scopeContext: new HttpContextScopeContext(getContextItems));
        }
    }

    /// <summary>Registers <see cref="DryIocHttpModule"/>.</summary>
    public static class DryIocHttpModuleInitializer
    {
        /// <summary>Registers once the type of <see cref="DryIocHttpModule"/>.</summary>
        public static void Initialize()
        {
            if (Interlocked.CompareExchange(ref _initialized, 1, 0) == 0)
                DynamicModuleUtility.RegisterModule(typeof(DryIocHttpModule));
        }

        private static int _initialized;
    }

    /// <summary>Hooks up <see cref="Container.OpenScope"/> on request beginning and scope dispose on request end.</summary>
    public sealed class DryIocHttpModule : IHttpModule
    {
        /// <summary>Initializes a module and prepares it to handle requests. </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        void IHttpModule.Init(HttpApplication context)
        {
            var scopeName = Reuse.WebRequestScopeName;

            context.BeginRequest += (sender, _) =>
            {
                var httpContext = (sender as HttpApplication).ThrowIfNull().Context;
                var scopeContext = new HttpContextScopeContext(() => httpContext.Items);

                // If current scope does not have WebRequestScopeName then create new scope with this name,
                // otherwise - use current.
                scopeContext.SetCurrent(current =>
                    current != null && scopeName.Equals(current.Name) ? current : new Scope(current, scopeName));
            };

            context.EndRequest += (sender, _) =>
            {
                var httpContext = (sender as HttpApplication).ThrowIfNull().Context;
                var scopeContext = new HttpContextScopeContext(() => httpContext.Items);

                var currentScope = scopeContext.GetCurrentOrDefault();
                if (currentScope != null && scopeName.Equals(currentScope.Name))
                    currentScope.Dispose();
            };
        }

        /// <summary>Disposes of the resources (other than memory) used by the module  that implements <see cref="IHttpModule"/>.</summary>
        void IHttpModule.Dispose() { }
    }

    /// <summary>Stores current scope in <see cref="HttpContext.Items"/>.</summary>
    /// <remarks>Stateless context, so could be created multiple times and used from different places without side-effects.</remarks>
    public sealed class HttpContextScopeContext : IScopeContext, IDisposable
    {
        /// <summary>Provides default context items dictionary using <see cref="HttpContext.Current"/>.
        /// Could be overridden with any key-value dictionary where <see cref="HttpContext"/> is not available, e.g. in tests.</summary>
        public static Func<IDictionary> GetContextItems = () =>
            HttpContext.Current.ThrowIfNull(Error.Of("No HttpContext is available.")).Items;

        /// <summary>Creates the context optionally with arbitrary/test items storage.</summary>
        /// <param name="getContextItems">(optional) Arbitrary/test items storage.</param>
        public HttpContextScopeContext(Func<IDictionary> getContextItems = null)
        {
            _currentScopeEntryKey = RootScopeName;
            _getContextItems = getContextItems ?? GetContextItems;
        }

        /// <summary>Fixed root scope name for the context.</summary>
        public static readonly string ScopeContextName = typeof(HttpContextScopeContext).FullName;

        /// <summary>Returns fixed name.</summary>
        public string RootScopeName { get { return ScopeContextName; } }

        /// <summary>Returns current ambient scope stored in item storage.</summary> <returns>Current scope or null if there is no.</returns>
        public IScope GetCurrentOrDefault()
        {
            return _getContextItems()[_currentScopeEntryKey] as IScope;
        }

        /// <summary>Sets the new scope as current using existing current as input.</summary>
        /// <param name="setCurrentScope">Delegate to get new scope.</param>
        /// <returns>Return new current scope.</returns>
        public IScope SetCurrent(SetCurrentScopeHandler setCurrentScope)
        {
            var newCurrentScope = setCurrentScope.ThrowIfNull()(GetCurrentOrDefault());
            _getContextItems()[_currentScopeEntryKey] = newCurrentScope;
            return newCurrentScope;
        }

        /// <summary>Nothing to dispose.</summary>
        public void Dispose() { }

        private readonly string _currentScopeEntryKey;
        private readonly Func<IDictionary> _getContextItems;
    }
}
