using MediatR;
using MediatRSample.Objects;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace MediatRSample.IoC_Configs
{
    class UnityConfig
    {
        internal static IUnityContainer BuildMediator()
        {
            var container = new UnityContainer();
            container.RegisterType<IMediator, Mediator>();
            container.RegisterTypes(AllClasses.FromAssemblies(typeof(Ping).Assembly), WithMappings.FromAllInterfaces, GetName, GetLifetimeManager);
            container.RegisterType(typeof(INotificationHandler<>), typeof(GenericHandler), GetName(typeof(GenericHandler)));
            container.RegisterType(typeof(IAsyncNotificationHandler<>), typeof(GenericAsyncHandler), GetName(typeof(GenericAsyncHandler)));
            container.RegisterInstance(Console.Out);
            container.RegisterInstance<SingleInstanceFactory>(t => container.Resolve(t));
            container.RegisterInstance<MultiInstanceFactory>(t => container.ResolveAll(t));

            return container;
        }

        static bool IsNotificationHandler(Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(INotificationHandler<>) || x.GetGenericTypeDefinition() == typeof(IAsyncNotificationHandler<>)));
        }

        static LifetimeManager GetLifetimeManager(Type type)
        {
            return IsNotificationHandler(type) ? new ContainerControlledLifetimeManager() : null;
        }

        static string GetName(Type type)
        {
            return IsNotificationHandler(type) ? string.Format("HandlerFor" + type.Name) : string.Empty;
        }
    }
}
