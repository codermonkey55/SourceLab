using MassTransit;
using MassTransitSample.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;

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

        public void Run()
        {
            var _receiveEndpointName = ConfigurationManager.AppSettings["MassTransitSample.CourierExample.ReceiveEndpoint.Name"];
            var _baseUri = new Uri(ConfigurationManager.AppSettings["MassTransitSample.BaseUri"]);

            _exampleCourierActivity = new ExampleCourierActivity(_baseUri);

            //-> Register Execute Buses for workflow process.
            _activityExecuteBuses = new List<IBusControl>();
            _activityExecuteBuses.Add(_exampleCourierActivity.CreateExecuteBus());

            // Register Compensate Buses for fallback scenarios.
            _activityCompensateBuses = new List<IBusControl>();
            _activityCompensateBuses.Add(_exampleCourierActivity.CreateCompensateBus());

            _bus = BusInitializer.CreateBus(_baseUri, bc =>
            {
                bc.ReceiveEndpoint(_receiveEndpointName, rec =>
                {
                    rec.Handler<IExampleEvent>(msg => BeginWorkflow(msg));
                });
            });

        }

        internal CourierExample New()
        {
            return new CourierExample();
        }
    }
}
