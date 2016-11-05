using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransitSample.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace MassTransitSample
{
    internal sealed class CourierExample
    {
        CourierExample()
        {

        }

        IBusControl _bus;
        List<IBusControl> _activityExecuteBuses;
        List<IBusControl> _activityCompensateBuses;

        ExampleCourierActivity _exampleCourierActivity;

        internal void Run()
        {
            var _receiveEndpointName = ConfigurationManager.AppSettings["MassTransitSample.CourierExample.ReceiveEndpoint.Name"];
            var _baseUri = new Uri(ConfigurationManager.AppSettings["MassTransitSample.BaseUri"]);

            _exampleCourierActivity = new ExampleCourierActivity(_baseUri);

            //-> Register Execute Buses for workflow process.
            _activityExecuteBuses = new List<IBusControl>();
            _activityExecuteBuses.Add(_exampleCourierActivity.CreateExecuteBus());

            //-> Register Compensate Buses for fallback scenarios.
            _activityCompensateBuses = new List<IBusControl>();
            _activityCompensateBuses.Add(_exampleCourierActivity.CreateCompensateBus());

            _bus = BusInitializer.CreateBus(_baseUri, bc =>
            {
                bc.ReceiveEndpoint(_receiveEndpointName, rec =>
                {
                    rec.Handler<IExampleEvent>(msgCtx => BeginWorkflow(msgCtx));
                });
            });
        }

        Task BeginWorkflow(ConsumeContext<IExampleEvent> exampleEvent)
        {
            return Task.Run(() =>
            {
                var builder = new RoutingSlipBuilder(NewId.NextGuid());

                builder.AddActivity(_exampleCourierActivity.ActivityName, _exampleCourierActivity.GetExecuteUri(), exampleEvent);

                RoutingSlip routingSlip = builder.Build();

                _bus.Execute(routingSlip);
            });
        }

        internal void Stop()
        {
            _bus = null;

            _activityExecuteBuses.Clear();
            _activityCompensateBuses.Clear();
        }

        internal static CourierExample New()
        {
            return new CourierExample();
        }
    }
}
