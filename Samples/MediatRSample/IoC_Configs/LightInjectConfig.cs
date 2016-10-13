using LightInject;
using MediatR;
using MediatRSample.Objects;
using System;
using System.Reflection;

namespace MediatRSample.IoC_Configs
{
    class LightInjectConfig
    {
        internal static IServiceContainer BuildMediator()
        {
            var serviceContainer = new ServiceContainer();
            serviceContainer.Register<IMediator, Mediator>();
            serviceContainer.RegisterAssembly(typeof(Ping).GetTypeInfo().Assembly, (serviceType, implementingType) => !serviceType.GetTypeInfo().IsClass);
            serviceContainer.RegisterAssembly(typeof(IMediator).GetTypeInfo().Assembly, (serviceType, implementingType) => !serviceType.GetTypeInfo().IsClass);
            serviceContainer.RegisterInstance(Console.Out);
            serviceContainer.Register<SingleInstanceFactory>(fac => t => fac.GetInstance(t));
            serviceContainer.Register<MultiInstanceFactory>(fac => t => fac.GetAllInstances(t));
            return serviceContainer;
        }
    }
}
