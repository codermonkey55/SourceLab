using System;
using System.Timers;
using Quartz;
using Topshelf;
using Topshelf.Quartz;
using TopshelfSample.Service;

namespace TopshelfSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host1TopshelfOnly = HostFactory.Run(host1Config =>
            {
                host1Config.Service<ExampleService>(service1Config =>
                {
                    service1Config.ConstructUsing(service => new ExampleService());

                    service1Config.WhenStarted(s => s.Start());
                    service1Config.WhenStarted((s, hostControl) => s.Start(hostControl));

                    service1Config.WhenStopped(s => s.Stop());
                    service1Config.WhenStopped((s, hostControl) => s.Stop(hostControl));

                    service1Config.WhenPaused(s => s.Pause());

                    service1Config.WhenContinued(s => s.Continue());

                    service1Config.WhenShutdown(s => s.Shutdown());
                });

                host1Config.RunAsLocalSystem();
                host1Config.SetServiceName("ExampleService");
            });

            var host2TOpshelfWithQuartz = HostFactory.Run(host2Config =>
            {
                host2Config.Service<TownCrier>(service2Config =>
                {
                    service2Config.ConstructUsing(name => new TownCrier());

                    service2Config.WhenStarted(tc => tc.Start());

                    service2Config.WhenStopped(tc => tc.Stop());

                    service2Config.ScheduleQuartzJob<TownCrier>(q =>
                        q.WithJob(() =>
                            JobBuilder.Create<ExampleJob>().Build())
                        .AddTrigger(() =>
                            TriggerBuilder.Create().WithIdentity(new TriggerKey("Name", "Group"))
                                .WithSimpleSchedule(builder => builder
                                    .WithIntervalInSeconds(20)
                                    .RepeatForever())
                                .Build())
                    );

                    service2Config.ScheduleQuartzJob<TownCrier>(q =>
                        q.WithJob(() =>
                            JobBuilder.Create<ExampleJob>().Build())
                        .AddTrigger(() =>
                            TriggerBuilder.Create().WithIdentity(new TriggerKey("Name", "Group"))
                                .WithSimpleSchedule(builder => builder
                                    .WithIntervalInSeconds(60)
                                    .RepeatForever())
                                .Build())
                        );
                });

                host2Config.RunAsLocalSystem();
                host2Config.SetServiceName("Stuff");
                host2Config.SetDisplayName("Stuff");
                host2Config.SetDescription("Sample Topshelf Host");
            });
        }
    }

    internal class TownCrier
    {
        private readonly Timer _timer;

        public TownCrier()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed +=
                (sender, eventArgs) =>
                    Console.WriteLine("It is {0} and all is well. By the way, it's time to go get your license.",
                        DateTime.Now);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}