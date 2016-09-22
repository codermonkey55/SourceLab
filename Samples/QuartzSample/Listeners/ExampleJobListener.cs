using Quartz;

namespace QuartzSample.Listeners
{
    public class ExampleJobListener : IJobListener
    {
        public string Name { get; }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            throw new System.NotImplementedException();
        }
    }
}
