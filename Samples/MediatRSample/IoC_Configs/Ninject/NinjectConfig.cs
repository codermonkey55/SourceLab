using MediatR;
using MediatRSample.Objects;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Planning.Bindings.Resolvers;
using System;
using System.IO;

namespace MediatRSample.IoC_Configs
{
    class NinjectConfig
    {
        internal static IKernel BuildMediator()
        {
            var kernel = new StandardKernel();
            kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
            kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>().SelectAllClasses().BindDefaultInterface());
            kernel.Bind(scan => scan.FromAssemblyContaining<Ping>().SelectAllClasses().BindAllInterfaces());
            kernel.Bind<TextWriter>().ToConstant(Console.Out);
            kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

            return kernel;
        }
    }
}
