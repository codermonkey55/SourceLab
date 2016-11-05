using System;

using MassTransit;
using MassTransit.Courier;

namespace MassTransitSample.Messaging
{
    public interface ICourierActivity<in TArguments, in TLog> : Activity<TArguments, TLog>
        where TArguments : class
        where TLog : class
    {
        string ActivityName { get; }

        IBusControl CreateExecuteBus(Uri baseUri);

        IBusControl CreateCompensateBus(Uri baseUri);
    }
}
