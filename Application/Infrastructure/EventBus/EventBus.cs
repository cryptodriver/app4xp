using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Infrastructure.EventBus
{
    /// <summary>
    /// The EventBus class is a simple and fast IEventBus implemention which processes event in the delivery order.
    /// </summary>
    /// <remarks>If you do not need the event processed in the delivery order, use SimpleEventBus instead.</remarks>
    public class EventBus : IEventBus, IDisposable
    {
        public EventBusConfig Config { set; get; }

        private bool _isDisposed;

        private readonly object _eventHandlerLock = new object();

        private readonly BlockingCollection<object> _eventQueue;

        private List<EventHandlerHolder> _eventHandlerList = new List<EventHandlerHolder>();

        /// <summary>
        /// The pending event number which does not yet dispatched.
        /// </summary>
        public int PendingEventNumber => Math.Max(_eventQueue.Count, 0);

        /// <summary>
        /// The constructor of EventBus.
        /// </summary>
        /// <param name="config">Configuration of instance</param>
        public EventBus(EventBusConfig config = null)
        {
            Config = config ?? new EventBusConfig();

            _eventQueue = new BlockingCollection<object>(Config.maxPendingEventNumber);

            Thread dispatchThread = new Thread(DispatchMessage) { IsBackground = true };
            dispatchThread.Name = "EventBus-Thread-" + dispatchThread.ManagedThreadId;
            dispatchThread.Start();

            Register(Config.handler);
        }

        /// <summary>
        /// Post an event to the event bus, dispatched after the specific time.
        /// </summary>
        /// <remarks>If you do not need the event processed in the delivery order, use SimpleEventBus instead.</remarks>
        /// <param name="package">The event object</param>
        /// <param name="delay">The delay seconds before dispatch this event</param>
        public void Post(object package, int delay = 0)
        {
            int delayMs = delay * 1000;

            if (delayMs >= 1)
            {
                Task.Delay(delayMs).ContinueWith(task => _eventQueue.Add(package));
            }
            else
            {
                _eventQueue.Add(package);
            }
        }

        /// <summary>
        /// Register event handlers in the handler instance.
        /// 
        /// One handler instance may have many event handler methods.
        /// These methods have EventSubscriberAttribute contract and exactly one parameter.
        /// </summary>
        /// <remarks>If you do not need the event processed in the delivery order, use SimpleEventBus instead.</remarks>
        /// <param name="handler">The instance of event handler class</param>
        public void Register(object handler)
        {
            if (handler == null)
            {
                return;
            }

            MethodInfo[] miList = handler.GetType().GetMethods();
            lock (_eventHandlerLock)
            {
                // Don't allow register multiple times.
                if (_eventHandlerList.Any(record => record.Handler == handler))
                {
                    return;
                }

                List<EventHandlerHolder> newList = null;
                foreach (MethodInfo mi in miList)
                {
                    EventSubscriberAttribute attribute = mi.GetCustomAttribute<EventSubscriberAttribute>();
                    if (attribute != null)
                    {
                        ParameterInfo[] piList = mi.GetParameters();
                        if (piList.Length == 1)
                        {
                            // OK, we got valid handler, create newList as needed
                            if (newList == null)
                            {
                                newList = new List<EventHandlerHolder>(_eventHandlerList);
                            }
                            newList.Add(new EventHandlerHolder(handler, mi, piList[0].ParameterType));
                        }
                    }
                }

                // OK, we have new handler registered
                if (newList != null)
                {
                    _eventHandlerList = newList;
                }
            }
        }

        /// <summary>
        /// Unregister event handlers belong to the handler instance.
        /// 
        /// One handler instance may have many event handler methods.
        /// These methods have EventSubscriberAttribute contract and exactly one parameter.
        /// </summary>
        /// <param name="handler">The instance of event handler class</param>
        public void Unregister(object handler)
        {
            if (handler == null)
            {
                return;
            }

            lock (_eventHandlerLock)
            {
                bool needAction = _eventHandlerList.Any(record => record.Handler == handler);

                if (needAction)
                {
                    List<EventHandlerHolder> newList = _eventHandlerList.Where(record => record.Handler != handler).ToList();
                    _eventHandlerList = newList;
                }
            }
        }

        private void DispatchMessage()
        {
            while (true)
            {
                object package = null;

                try
                {
                    package = _eventQueue.Take();
                    InvokeEventHandler(package);
                }
                catch (Exception de)
                {
                    if (de is ObjectDisposedException)
                    {
                        return;
                    }

                    Trace.TraceError("Dispatch event ({0}) failed: {1}{2}{3}",
                        package, de.Message, Environment.NewLine, de.StackTrace);
                }
            }
        }

        private void InvokeEventHandler(object package)
        {
            List<Task> taskList = null;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _eventHandlerList.Count; i++)
            {
                // ReSharper disable once InconsistentlySynchronizedField
                EventHandlerHolder record = _eventHandlerList[i];
                if (package == null || record.ParameterType.IsInstanceOfType(package))
                {
                    Task task = Task.Run(() => record.MethodInfo.Invoke(record.Handler, new[] { package }));
                    if (taskList == null) taskList = new List<Task>();
                    taskList.Add(task);
                }
            }
            if (taskList != null)
            {
                Task.WaitAll(taskList.ToArray());
            }
        }

        /// <summary>
        /// Releases resources used by the <see cref="EventBus"/> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources used by the <see cref="EventBus"/> instance.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _eventQueue.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
