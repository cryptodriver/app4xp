using System;
using System.Threading;

namespace Application.Infrastructure.ThreadPool
{
    public class Work : IDisposable
    {
        public bool IsWorking { get; set; }

        public object Context { get; set; }

        public string Key { get; set; }

        public Thread Thread { get; set; }

        public AutoResetEvent Locker { get; set; }

        public WaitCallback Callback { get; set; }

        public event Action<Work> Completed;

        public Work()
        {
            Locker = new AutoResetEvent(false);

            Key = Guid.NewGuid().ToString();

            Thread = new Thread(() =>
            {
                while (IsWorking)
                {
                    Locker.WaitOne();
                    Callback(Context);
                    Completed(this);
                }
            })
            {
                IsBackground = true
            };

            IsWorking = true;

            Context = new object();

            Thread.Start();
        }

        public void Active()
        {
            IsWorking = true;
            Locker.Set();
        }

        public void Close()
        {
            IsWorking = false;
        }

        public void Dispose()
        {
            try
            {
                Thread.Abort();
            }
            catch (Exception)
            {
                return;
            }
        }

        public void SetWorkItem(WaitCallback callback, object context)
        {
            Callback = callback;
            Context = context;
        }
    }
}
