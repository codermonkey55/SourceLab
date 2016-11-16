using MassTransit.Courier;
using MassTransitSample.Messaging;
using System;

namespace MassTransitSample
{
    internal class ExampleCourierActivity : ActivityBase<ExampleCourierActivity, IExampleArguments, IExampleLog>
    {
        public ExampleCourierActivity() { }

        public ExampleCourierActivity(Uri baseUri) : base(baseUri) { }

        public override string ActivityName
        {
            get
            {
                return "ExampleCourierActivity";
            }
        }

        public override ExecutionResult Execute(ExecuteContext<IExampleArguments> execution)
        {
            throw new NotImplementedException();
        }

        public override CompensationResult Compensate(CompensateContext<IExampleLog> compensation)
        {
            throw new NotImplementedException();
        }
    }
}
