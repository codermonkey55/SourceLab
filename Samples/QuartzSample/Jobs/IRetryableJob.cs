using System;
using Quartz;

namespace QuartzSample.Jobs
{
    public interface IRetryableJob : IJob
    {
        int MaxNumberTries { get; }
        DateTime StartTimeRetryUtc { get; }
        DateTime? EndTimeRetryUtc { get; }
    }
}
