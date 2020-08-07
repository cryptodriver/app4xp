using System.Collections.Generic;
using System.Threading;

namespace Application.Infrastructure.ThreadPool
{
    public interface IThreadPool
    {
        void SetMaxThread(int Value);

        void SetMinThread(int Value);

        void SetIncrement(int Value);

        int AddWorkItem(WorkItem workItem);

        int AddWorkItem(WaitCallback callBack, object context);

        void CloseThread(int threadId);

        Work GetWork(int threadId);

        List<Work> GetWorks();

        void Dispose();
    }
}
