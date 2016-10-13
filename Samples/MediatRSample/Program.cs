using Autofac;
using DryIoc;
using MediatR;
using MediatRSample.IoC_Configs;
using MediatRSample.Objects;
using Microsoft.Practices.Unity;
using System;

namespace MediatRSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var autofacContainer = AutofacConfig.BuildMediator();
            var mediator = autofacContainer.Resolve<IMediator>();

            var dryIocContainer = DryIocConfig.BuildMediator();
            var mediator2 = dryIocContainer.Resolve<IMediator>();

            var lightInjectContainer = LightInjectConfig.BuildMediator();
            var mediator3 = lightInjectContainer.GetInstance<IMediator>();

            //var ninjectContainer = NinjectConfig.BuildMediator();
            //var mediator4 = ninjectContainer.Get<IMediator>();

            var simpleInjectorContainer = SimpleInjectorConfig.BuildMediator();
            var mediator5 = simpleInjectorContainer.GetInstance<IMediator>();

            var structureMapContainer = StructureMapConfig.BuildMediator();
            var mediator6 = structureMapContainer.GetInstance<IMediator>();

            var unityContainer = UnityConfig.BuildMediator();
            var mediator7 = unityContainer.Resolve<IMediator>();

            var windsorContainer = WindsorConfig.BuildMediator();
            var mediator8 = windsorContainer.Resolve<IMediator>();

            Runner.Run(mediator, Console.Out);

            Console.ReadKey();
        }
    }
}
