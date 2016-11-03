using MassTransit.Courier;
using System.Collections.Generic;

namespace MassTransitSample.Messaging.Extensions
{
    public static class ExcutionExtensions
    {
        public static ExecutionResult CancelRoutingSlip<TArguments, TLog>(this ExecuteContext<TArguments> execution, TLog tLog, string cancelReason)
            where TArguments : class
            where TLog : class
        {
            Dictionary<string, object> variables = new Dictionary<string, object>();

            variables.Add(EventProcessingWorkflow.State, EventProcessingWofklowState.Cancelled);

            variables.Add(EventProcessingWorkflow.CancelReason, cancelReason);

            return execution.ReviseItinerary(tLog, variables, itnr => { });
        }

        public static ExecutionResult SetRoutingSlipAsCompleted<TArguments>(this ExecuteContext<TArguments> execution, IEventProcessingPayload payload)
            where TArguments : class
        {
            Dictionary<string, object> variables = new Dictionary<string, object>();

            variables.Add(EventProcessingWorkflow.State, EventProcessingWofklowState.Completed);

            variables.Add(EventProcessingWorkflow.ProcessedPayload, payload);

            return execution.ReviseItinerary(default(object), variables, itnr => { });
        }

    }
}
