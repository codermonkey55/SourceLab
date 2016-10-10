using DryIoc;
using DryIoc.Web;
using System.Web;

namespace DIWebAppSample.IoC_Core.DryIoc.Web
{
    public sealed class DryIocWebHttpModule : IHttpModule
    {
        /// <summary>Initializes a module and prepares it to handle requests. </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += (sender, _) =>
            {
                var httpContext = (sender as HttpApplication).ThrowIfNull().Context;
                var scopeContext = new HttpContextScopeContext(() => httpContext.Items);

                //scopeContext.OpenScope();
                //.ThrowIf(s => s.Parent != null, Error.ROOT_SCOPE_IS_ALREADY_OPENED);
            };

            context.EndRequest += (sender, _) =>
            {
                var httpContext = (sender as HttpApplication).ThrowIfNull().Context;
                var scopeContext = new HttpContextScopeContext(() => httpContext.Items);

                var scope = scopeContext.GetCurrentOrDefault();
                //.ThrowIfNull(Erro.NO_OPENED_SCOPE_TO_DISPOSE)
                //.ThrowIf(s => s.Parent != null, Error.NOT_THE_ROOT_OPENED_SCOPE);

                scope.Dispose();
            };
        }

        /// <summary>Disposes of the resources (other than memory) used by the module  that implements <see cref="IHttpModule"/>.</summary>
        void IHttpModule.Dispose() { }
    }
}