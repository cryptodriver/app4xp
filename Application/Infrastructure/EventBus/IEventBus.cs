namespace Application.Infrastructure.EventBus
{
    public interface IEventBus
    {
        void Post(object eventObject, int delay = 0);

        void Register(object handler);

        void Unregister(object handler);
    }
}
