using Application;
using Application.Infrastructure.Logger;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace Common.Helpers
{
    public class LoggerHelper
    {
        private static ILogger _logger;

        static LoggerHelper()
        {
            _logger = ServiceProvider.Instance.Get<ILoggerManager>().GetLogger();
        }

        public static void Write(string _message = "*", LogLevel _level = LogLevel.INFO)
        {
            var _id = Thread.CurrentThread.ManagedThreadId;
            var _for = "[{0}]:{1} - {2}";

            switch (_level)
            {
                case LogLevel.DEBUG:
                    _logger.Debug(string.Format(_for, _id, _GetCaller(), _message));
                    break;
                case LogLevel.INFO:
                    _logger.Info(string.Format(_for, _id, _GetCaller(), _message));
                    break;
                case LogLevel.WARN:
                    _logger.Warn(string.Format(_for, _id, _GetCaller(), _message));
                    break;
                case LogLevel.ERROR:
                    _logger.Error(string.Format(_for, _id, _GetCaller(), _message));
                    break;
                case LogLevel.FATAL:
                    _logger.Fatal(string.Format(_for, _id, _GetCaller(), _message));
                    break;
            }
        }

        private static string _GetCaller()
        {
            Type _type;
            MethodBase _method;
            string _name = string.Empty;
            int _line = -1;

            int _skip = 2;
            do
            {
                var _stack = new StackFrame(_skip, true);
                _method = _stack.GetMethod();
                _type = _method.DeclaringType;

                if (_type == null) break;

                if (_type.FullName.IndexOf("+") > 0)
                {
                    _name = _type.FullName;
                }
                else
                {
                    var _temp = _method.Name;
                    if (new Regex(@"<.+>").IsMatch(_temp))
                    {
                        _temp = _temp.Substring(_temp.IndexOf("<") + 1, _temp.IndexOf(">") - 1);
                    }
                    _name = string.Format("{0}+<{1}>", _type.FullName, _temp);
                }

                _line = _stack.GetFileLineNumber();

                _skip++;
            }
            while (_type.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            _name = _name.Substring(0, _name.IndexOf(">") + 1);

            return string.Format("{0}#{1}", _name, _line);
        }
    }
}
