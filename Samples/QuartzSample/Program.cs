using Quartz;
using Quartz.Impl;
using QuartzSample.Jobs;
using System;
using System.Configuration;

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
            ICronTrigger trigger = TriggerBuilder.Create()
                                                 .WithIdentity("WriteHelloToLog", "IT")
                                                 .WithCronSchedule("0 0/1 * 1/1 * ? *") //-> visit http://www.cronmaker.com/ Queues the job every minute
                                                 .StartAt(DateTime.UtcNow)
                                                 .WithPriority(1)
                                                 .Build() as ICronTrigger;

            ITrigger dailyTrigger = TriggerBuilder.Create()
                                      .WithDailyTimeIntervalSchedule(schedBuilder => schedBuilder
                                          .WithIntervalInMinutes(30)
                                          .OnDaysOfTheWeek(new[] {
                                                          DayOfWeek.Monday,
                                                          DayOfWeek.Tuesday,
                                                          DayOfWeek.Wednesday,
                                                          DayOfWeek.Thursday,
                                                          DayOfWeek.Friday })
                                          .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(9, 30))
                                          .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(19, 30)))
                                      .Build();

            scheduler.ScheduleJob(jobDetail, trigger);

            scheduler.TriggerJob(new JobKey("ExampleJob"));
        }
    }
}
