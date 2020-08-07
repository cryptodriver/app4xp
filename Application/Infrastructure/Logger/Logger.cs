namespace Application.Infrastructure.Logger
{
    public class Logger : ILogger
    {
        private readonly ILoggerWriter _writer;

        private readonly LoggerConfig _config;

        public Logger(LoggerConfig config)
        {
            _config = config;
            _writer = new LoggerWriter(config.LogFile);
        }

        public void Debug(string message) => WriteLog(LogLevel.DEBUG, message);

        public void Error(string message) => WriteLog(LogLevel.ERROR, message);

        public void Fatal(string message) => WriteLog(LogLevel.FATAL, message);

        public void Info(string message) => WriteLog(LogLevel.INFO, message);

        public void Warn(string message) => WriteLog(LogLevel.WARN, message);

        protected void WriteLog(LogLevel level, string message)
        {
            if (level >= _config.Level)
            {
                _writer.Write(new LoggerRecord(level, message));
            }
        }
    }
}
