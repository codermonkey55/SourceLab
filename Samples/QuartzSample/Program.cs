using System;
using System.Configuration;
using Quartz;
using Quartz.Impl;
using QuartzSample.Jobs;

namespace QuartzSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScheduledJob.Run();
            // TODO: Initialiaze Scheduler
            var scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.Start();

            var jobDetail = JobBuilder.Create<ExampleJob>()
                .WithIdentity(new JobKey("ExampleJob"))
                .Build();
            jobDetail.JobDataMap.Put("DataKey", "Passed value");
            jobDetail.JobDataMap.Put("FilePath", ConfigurationManager.AppSettings["FilePath"] as string);

            // Let's create a trigger that fires immediately
            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("WriteHelloToLog", "IT")
                .WithCronSchedule("0 0/1 * 1/1 * ? *") // visit http://www.cronmaker.com/ Queues the job every minute
                .StartAt(DateTime.UtcNow)
                .WithPriority(1)
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);

            scheduler.TriggerJob(new JobKey("ExampleJob"));
        }
    }
}
