using System;

namespace Application.Infrastructure.Logger
{
    public class LoggerManager : ILoggerManager
    {
        private readonly Lazy<ILogger> _logger;

        public LoggerManager(LoggerConfig config)
        {
            if (config == null)
            {
                config = new LoggerConfig();
            }

            _logger = new Lazy<ILogger>(() => new Logger(config));
        }

        public ILogger GetLogger() => _logger.Value;
    }
}
