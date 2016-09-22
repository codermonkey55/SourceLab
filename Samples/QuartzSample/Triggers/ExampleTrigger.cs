using System;
using Quartz;

namespace QuartzSample.Triggers
{
    internal class ExampleTrigger : ITrigger
    {
        private DateTime? _endTimeRetryUtc;
        private TriggerKey _key;
        private DateTime _startTimeRetryUtc;
        private int _v;
        private TimeSpan _zero;

        public ExampleTrigger(TriggerKey key, DateTime startTimeRetryUtc, DateTime? endTimeRetryUtc, int v, TimeSpan zero)
        {
            this._key = key;
            this._startTimeRetryUtc = startTimeRetryUtc;
            this._endTimeRetryUtc = endTimeRetryUtc;
            this._v = v;
            this._zero = zero;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(ITrigger other)
        {
            throw new NotImplementedException();
        }

        public TriggerBuilder GetTriggerBuilder()
        {
            throw new NotImplementedException();
        }

        public IScheduleBuilder GetScheduleBuilder()
        {
            throw new NotImplementedException();
        }

        public bool GetMayFireAgain()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset? GetNextFireTimeUtc()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset? GetPreviousFireTimeUtc()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset? GetFireTimeAfter(DateTimeOffset? afterTime)
        {
            throw new NotImplementedException();
        }

        public TriggerKey Key { get; }
        public JobKey JobKey { get; }
        public string Description { get; }
        public string CalendarName { get; }
        public JobDataMap JobDataMap { get; }
        public DateTimeOffset? FinalFireTimeUtc { get; }
        public int MisfireInstruction { get; }
        public DateTimeOffset? EndTimeUtc { get; }
        public DateTimeOffset StartTimeUtc { get; }
        public int Priority { get; set; }
        public bool HasMillisecondPrecision { get; }
    }
}