using System.IO;
using System.Text;

namespace Application.Infrastructure.Logger
{
    public class LoggerWriter : ILoggerWriter
    {
        private readonly string _file;

        private static readonly object locker = new object();

        public void Write(ILoggerRecord message)
        {
            lock (locker)
            {
                using (StreamWriter sw = new StreamWriter(_file, true, Encoding.Default))
                {
                    sw.WriteLine(message.ToString());
                }
            }
        }

        public LoggerWriter(string fileName) => this._file = fileName;
    }
}
