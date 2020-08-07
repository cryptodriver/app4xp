namespace Application.Infrastructure.Logger
{
    public interface ILoggerWriter
    {
        void Write(ILoggerRecord message);
    }
}
