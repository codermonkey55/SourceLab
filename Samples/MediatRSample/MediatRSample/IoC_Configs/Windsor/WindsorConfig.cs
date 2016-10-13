using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MediatR;
using MediatRSample.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace MediatRSample.IoC_Configs
{
    class WindsorConfig
    {
        internal static IWindsorContainer BuildMediator()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IMediator>().ImplementedBy<Mediator>());
            container.Register(Classes.FromAssemblyContaining<Ping>().Pick().WithServiceAllInterfaces());
            container.Register(Component.For<TextWriter>().Instance(Console.Out));
            container.Kernel.AddHandlersFilter(new ContravariantFilter());
            container.Register(Component.For<SingleInstanceFactory>().UsingFactoryMethod<SingleInstanceFactory>(k => t => k.Resolve(t)));
            container.Register(Component.For<MultiInstanceFactory>().UsingFactoryMethod<MultiInstanceFactory>(k => t => (IEnumerable<object>)k.ResolveAll(t)));

            return container;
        }
    }
}
