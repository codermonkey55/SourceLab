using System;
using System.Threading.Tasks;

using Topshelf;

namespace TopshelfSample.Service
{
    internal class ExampleService : ServiceControl
    {
        private readonly Task _task;

        private HostControl _hostControl;

        public ExampleService()
        {
            _task = new Task(DoWork);
        }

        private void DoWork()
        {
            Console.WriteLine("Listen very carefully, I shall say this only once.");

            _hostControl.Stop();
        }

        public void Start() { }

        public void Stop() { }

        public void Pause() { }

        public void Continue() { }

        public void Shutdown() { }

        public Boolean Start(HostControl hostControl)
        {
            // So we can stop the service at the end of the check.
            this._hostControl = hostControl;

            // Start the DoWork thread.
            this._task.Start();

            // If the task takes longer than expected then request more time to start the service.
            this._hostControl.RequestAdditionalTime(new TimeSpan(1));

            return true;
        }

        public Boolean Stop(HostControl hostControl)
        {
            return true;
        }
    }
}
