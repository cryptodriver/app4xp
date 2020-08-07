namespace Application.Infrastructure.EventBus
{
    public class EventBusConfig
    {
        public int maxPendingEventNumber { set; get; } = 1024 * 1024;

        public object handler { get; set; }

    }
}
