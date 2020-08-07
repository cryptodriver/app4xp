using System;
using System.Collections.Generic;
using System.Threading;

namespace Application.Infrastructure.ThreadPool
{
    public class ThreadPool : IThreadPool, IDisposable
    {
        private int max = 64;

        private int min = 10;

        private int increment = 2;

        private List<string> keys;

        private Queue<Work> fqueue;

        private Queue<WorkItem> wqueue;

        private Dictionary<string, Work> working;

        public Dictionary<string, Work> Pool { get; set; }

        //private static ThreadPool manager;

        public void SetMaxThread(int Value)
        {
            lock (this)
            {
                max = Value;
            }
        }

        public void SetMinThread(int Value)
        {
            lock (this)
            {
                min = Value;
            }
        }

        public void SetIncrement(int Value)
        {
            lock (this)
            {
                increment = Value;
            }
        }

        public ThreadPool()
        {
            Initialize();
        }

        private ThreadPool(int min, int max, int increment = 1)
        {
            this.min = min;
            this.max = max;
            this.increment = increment;

            Initialize();
        }

        private void Initialize()
        {
            Pool = new Dictionary<string, Work>();
            working = new Dictionary<string, Work>();
            keys = new List<string>();

            fqueue = new Queue<Work>();
            wqueue = new Queue<WorkItem>();

            Work work = null;
            for (int i = 0; i < min; i++)
            {
                work = new Work();
                work.Completed += Completed;
                Pool.Add(work.Key, work);
                fqueue.Enqueue(work);
            }
        }

        private void Completed(Work work)
        {
            lock (this)
            {
                working.Remove(work.Key);

                if (wqueue.Count > 0)
                {
                    Pool.Remove(work.Key);
                    work.Close();

                    WorkItem item = wqueue.Dequeue();
                    if (fqueue.Count > 0)
                    {
                        work = fqueue.Dequeue();
                    }
                    else
                    {
                        work = new Work();
                        work.Completed += Completed;
                        Pool.Add(work.Key, work);
                    }

                    work.Callback = item.Callback;
                    work.Context = item.Context;

                    working.Add(work.Key, work);

                    keys.Add(work.Key);
                    work.Active();
                }
                else
                {
                    if (fqueue.Count > min)
                    {
                        Pool.Remove(work.Key);
                        work.Dispose();
                    }
                    else
                    {
                        work.Context = null;
                        fqueue.Enqueue(work);
                    }
                }
            }
        }

        public int AddWorkItem(WorkItem workItem)
        {
            return AddWorkItem(workItem.Callback, workItem.Context);
        }

        public int AddWorkItem(WaitCallback callBack, object context)
        {
            lock (this)
            {
                Work work = null;
                int len = Pool.Values.Count;

                if (fqueue.Count == 0)
                {
                    if (len < max)
                    {
                        for (int i = 0; i < increment; i++)
                        {
                            if ((len + i + 1) <= max)
                            {
                                work = new Work();
                                work.Completed += Completed;

                                fqueue.Enqueue(work);
                                Pool.Add(work.Key, work);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        wqueue.Enqueue(new WorkItem { Callback = callBack, Context = context });

                        return -1;
                    }
                }

                work = fqueue.Dequeue();
                work.SetWorkItem(callBack, context);
                work.Active();

                working.Add(work.Key, work);
                keys.Add(work.Key);

                return work.Thread.ManagedThreadId;
            }
        }

        public void CloseAfterCurrentWork(int threadId)
        {
            Work work = GetWork(threadId);
            if (work != null)
            {
                work.Close();
            }
        }

        public void CloseThread(int threadId)
        {
            Work work = GetWork(threadId);
            working.Remove(work.Key);
            keys.Remove(work.Key);
            fqueue.Enqueue(work);
            work.Locker.WaitOne();
        }

        public Work GetWork(int threadId)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                Work n = working.ContainsKey(key) ? working[key] : null;
                if (n != null && n.Thread.ManagedThreadId == threadId)
                {
                    return n;
                }
            }

            return null;
        }

        public void Dispose()
        {
            foreach (Work w in Pool.Values)
            {
                using (w) { w.Close(); }
            }

            Pool.Clear();
            working.Clear();
            keys.Clear();
            wqueue.Clear();
            fqueue.Clear();
        }

        public List<Work> GetWorks()
        {
            var works = new List<Work>();

            lock (this)
            {
                foreach (KeyValuePair<string, Work> wk in working)
                {
                    works.Add(wk.Value);
                }
            }

            return works;
        }
    }
}