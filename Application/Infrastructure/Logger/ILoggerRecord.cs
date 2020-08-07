using System;

namespace Application.Infrastructure.Logger
{
    public interface ILoggerRecord
    {
        LogLevel Level { get; }
        DateTime Timestamp { get; }
        string Message { get; }
        string ToString();
    }
}
