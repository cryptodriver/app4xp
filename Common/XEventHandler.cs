using Application;
using Application.Infrastructure.EventBus;
using Application.Interfaces;
using Common.Dispachers;
using Common.Helpers;
using System.Dynamic;
using System.Threading;

namespace Common
{
    public class XEventHandler : IEventHandler
    {
        [EventSubscriber]
        public void HandleEvent(EventPackage package)
        {
            LoggerHelper.Write(string.Format("params: {0}", package?.ToString()));

            IDispacher dispacher;
            switch (package.Tag)
            {
                case EventTag.TASK:
                    dispacher = ServiceProvider.Instance.Get<ITaskDispacher>();
                    break;
                case EventTag.ERROR:
                    dispacher = ServiceProvider.Instance.Get<IErrorDispacher>();
                    break;
                default:
                    dispacher = ServiceProvider.Instance.Get<IDispacher>();
                    break;
            }

            dynamic _ctx = new ExpandoObject();

            _ctx.Payload = package.Payload;
            _ctx.CTSource = new CancellationTokenSource();

            dispacher.Dispach(_ctx);
        }
    }
}
