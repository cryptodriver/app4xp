using System;
using System.IO;

namespace Application.Infrastructure.Logger
{
    public class LoggerConfig
    {
        public string FileName { get; set; } = $"{DateTime.Now.ToString("yyyyMMdd")}.log";

        public string FilePath { get; set; }

        public string LogFile => Path.Combine(FilePath, FileName);

        public LogLevel Level { get; set; } = LogLevel.INFO;
    }
}
