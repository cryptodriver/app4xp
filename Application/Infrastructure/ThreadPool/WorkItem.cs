using System.Threading;

namespace Application.Infrastructure.ThreadPool
{
    public class WorkItem
    {
        public WaitCallback Callback { get; set; }

        public object Context { get; set; }
    }
}
