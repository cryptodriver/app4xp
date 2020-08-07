using System;

namespace Application.Infrastructure.Logger
{
    public class LoggerRecord : ILoggerRecord
    {
        public LogLevel Level { get; }

        public DateTime Timestamp { get; }

        public string Message { get; }

        public LoggerRecord(LogLevel level, string message)
        {
            Level = level;
            Message = message;
            Timestamp = DateTime.Now;
        }

        string ILoggerRecord.ToString()
        {
            return $"[{Timestamp.ToString()}][{Level}] - {Message}";
        }
    }
}
