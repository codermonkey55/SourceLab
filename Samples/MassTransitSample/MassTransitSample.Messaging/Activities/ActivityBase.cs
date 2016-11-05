using MassTransit;
using MassTransit.Courier;
using System;
using System.Threading.Tasks;

namespace MassTransitSample.Messaging
{
    public abstract class ActivityBase<TIActivity, TArguments, TLog> : ICourierActivity<TArguments, TLog>
        where TIActivity : class, ICourierActivity<TArguments, TLog>
        where TArguments : class
        where TLog : class
    {
        // Fields
        protected Type _tLogType;

        protected Type _activityType;

        protected Type _tArgumentsType;

        protected readonly Uri _baseUri;


        // Properties
        public abstract string ActivityName { get; }


        // Constructors
        public ActivityBase() { }

        public ActivityBase(Uri baseUri) { _baseUri = baseUri; _activityType = typeof(TIActivity); _tArgumentsType = typeof(TArguments); _tLogType = typeof(TLog); }


        // Methods
        public abstract ExecutionResult Execute(ExecuteContext<TArguments> execution);

        public abstract CompensationResult Compensate(CompensateContext<TLog> compensation);

        public IBusControl CreateExecuteBus() { return CreateExecuteBus(_baseUri); }

        public IBusControl CreateCompensateBus() { return CreateCompensateBus(_baseUri); }

        public IBusControl CreateExecuteBus(Uri baseUri)
        {
            return BusInitializer.CreateBus(baseUri, x =>
            {
                x.ReceiveEndpoint(ActivityName, sbc => sbc.ExecuteActivityHost<TIActivity, TArguments>(_ => default(TIActivity)));
            });
        }

        public IBusControl CreateCompensateBus(Uri baseUri)
        {
            return BusInitializer.CreateBus(baseUri, x =>
            {
                x.ReceiveEndpoint(ActivityName, sbc => sbc.CompensateActivityHost<TIActivity, TLog>(_ => default(TIActivity)));
            });
        }

        public Uri GetExecuteUri(Uri baseUri = null)
        {
            return GetExecuteUri(_baseUri ?? baseUri, ActivityName);
        }

        public Uri GetCompensateUri(Uri baseUri = null)
        {
            return GetCompensateUri(_baseUri ?? baseUri, ActivityName);
        }

        public static Uri GetExecuteUri(Uri baseUriParam, string activityNameParam)
        {
            var baseUri = baseUriParam ?? new Uri("");
            var activityName = activityNameParam ?? "";

            return new Uri(string.Format("{0}.{1}", baseUri.AbsoluteUri, activityName));
        }

        public static Uri GetCompensateUri(Uri baseUriParam, string activityNameParam)
        {
            var baseUri = baseUriParam ?? new Uri("");
            var activityName = activityNameParam ?? "";

            return new Uri(string.Format("{0}.{1}.Compensate", baseUri.AbsoluteUri, activityName));
        }


        Task<ExecutionResult> ExecuteActivity<TArguments>.Execute(ExecuteContext<TArguments> context)
        {
            throw new NotImplementedException();
        }

        Task<CompensationResult> CompensateActivity<TLog>.Compensate(CompensateContext<TLog> context)
        {
            throw new NotImplementedException();
        }
    }
}
