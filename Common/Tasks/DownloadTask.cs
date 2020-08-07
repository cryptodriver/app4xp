using Application;
using Application.Infrastructure.ThreadPool;
using Common.Helpers;
using System;
using System.Threading;

namespace Common.Tasks
{
    public class DownloadTask : IDownloadTask
    {
        public bool Run(dynamic context)
        {
            var pool = ServiceProvider.Instance.Get<IThreadPool>();

            pool.AddWorkItem(new WaitCallback(TaskExecutor), context);

            return true;
        }

        private void TaskExecutor(dynamic data)
        {
            // do task here
            System.Threading.Thread.Sleep((new Random()).Next(1000, 20000));

            LoggerHelper.Write("Task finished!");
        }
    }
}
