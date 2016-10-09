using System.Reflection;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Ninject.Modules
{
    /// <summary>
    /// <see cref="https://spin.atomicobject.com/2014/03/14/ninject-binding-kernel-cleanup/"/>
    /// <see cref="https://github.com/ninject/Ninject.Extensions.Conventions"/>
    /// </summary>
    internal class AutoControllerModule : NinjectModule
    {
        private readonly Assembly _assembly;

        public AutoControllerModule(Assembly assembly)
        {
            _assembly = assembly;
        }

        public override void Load()
        {
            Kernel.Bind(x =>
            {
                x.From(_assembly)
                 .SelectAllClasses()
                 .Where(type =>
                        type.Name.EndsWith("Controller") &&
                       (type.Namespace?.Contains("Controllers") ?? false))
                 .BindToSelf()
                 .Configure(b => b.InRequestScope());
            });
        }
    }

}