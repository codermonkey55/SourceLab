using Quartz;
using Quartz.Spi;

namespace QuartzSample.Factories
{
    public class ExampleJobFactory : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            throw new System.NotImplementedException();
        }

        public void ReturnJob(IJob job)
        {
            throw new System.NotImplementedException();
        }
    }
}
