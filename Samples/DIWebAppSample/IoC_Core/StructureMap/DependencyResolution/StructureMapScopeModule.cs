using System.Web;

using CodeLabs.Web.WebForms.IoC_Integration;

using StructureMap.Web.Pipeline;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.StructureMap.DependencyResolution
{
    public class StructureMapScopeModule : IHttpModule
    {
        #region Public Methods and Operators

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => StructuremapMvc.StructureMapDependencyScope.CreateNestedContainer();

            context.EndRequest += (sender, e) =>
            {
                HttpContextLifecycle.DisposeAndClearAll();
                StructuremapMvc.StructureMapDependencyScope.DisposeNestedContainer();
            };
        }

        #endregion
    }
}