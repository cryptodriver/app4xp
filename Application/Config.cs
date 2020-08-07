using Application.Helpers;
using Application.Infrastructure.EventBus;

namespace Application
{
    public class Config
    {
        public UIType CurrentUI { set; get; }

        public string UserAgent { set; get; }

        public string DeviceId { set; get; }

        public string DeviceName { set; get; }

        public int CurrentId { set; get; }

        public string CurrentAccount { set; get; }

        public string CurrentPassword { set; get; }

        public string CurrentPath { set; get; }

        public string DefaultLDBName { set; get; } = "ldb";

        public string LogLPath { set; get; } = "Log";

        public int DefaultLogLevel { set; get; }

        public IEventHandler EventHandler { set; get; }
    }
}
