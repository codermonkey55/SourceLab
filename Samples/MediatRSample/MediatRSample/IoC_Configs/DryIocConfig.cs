using DryIoc;
using MediatR;
using MediatRSample.Objects;
using System;
using System.Reflection;

namespace MediatRSample.IoC_Configs
{
    class DryIocConfig
    {
        internal static IContainer BuildMediator()
        {
            var container = new Container();

            container.RegisterDelegate<SingleInstanceFactory>(r => serviceType => r.Resolve(serviceType));
            container.RegisterDelegate<MultiInstanceFactory>(r => serviceType => r.ResolveMany(serviceType));
            container.RegisterInstance(Console.Out);

            container.RegisterMany(new[] { typeof(IMediator).GetAssembly(), typeof(Ping).GetAssembly() }, type => type.GetTypeInfo().IsInterface);

            return container;
        }
    }
}
