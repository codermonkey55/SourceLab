using System;
using Quartz;
using Quartz.Impl.Triggers;
using QuartzSample.Jobs;

namespace QuartzSample.Listeners
{
    public class RetryableJobListener : IJobListener
    {
        public const string NumberTriesJobDataMapKey = "RetryableJobListener.TryNumber";

        private readonly IScheduler _scheduler;

        public RetryableJobListener(IScheduler scheduler)
        {
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));

            _scheduler = scheduler;
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            var retryableJob = context.JobInstance as IRetryableJob;
            if (retryableJob == null)
                return;

            if (!context.JobDetail.JobDataMap.Contains(NumberTriesJobDataMapKey))
                context.JobDetail.JobDataMap[NumberTriesJobDataMapKey] = 0;

            int numberTries = context.JobDetail.JobDataMap.GetIntValue(NumberTriesJobDataMapKey);
            context.JobDetail.JobDataMap[NumberTriesJobDataMapKey] = ++numberTries;
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            if (jobException == null)
                return;

            var retryableJob = context.JobInstance as IRetryableJob;
            if (retryableJob == null)
                return;

            int numberTries = context.JobDetail.JobDataMap.GetIntValue(NumberTriesJobDataMapKey);
            if (numberTries >= retryableJob.MaxNumberTries)
                return; // Max number tries reached

            // Schedule next try
            ScheduleRetryableJob(context, retryableJob);
        }

        private void ScheduleRetryableJob(IJobExecutionContext context, IRetryableJob retryableJob)
        {
            var oldTrigger = context.Trigger;

            // Unschedule old trigger
            _scheduler.UnscheduleJob(oldTrigger.Key);

            // Create and schedule new trigge
            var retryTrigger = new SimpleTriggerImpl(oldTrigger.Key.Name, retryableJob.StartTimeRetryUtc, retryableJob.EndTimeRetryUtc, 0, TimeSpan.Zero);
            _scheduler.ScheduleJob(context.JobDetail, retryTrigger);
        }

        public string Name
        {
            get { return this.GetType().FullName; }
        }
    }
}