using Application;
using Application.Core;

namespace Common
{
    public class Bootstrapper
    {

        public static void Initialize(Config config)
        {
            XSetting.Init(config);

            config.EventHandler = new XEventHandler();

            ServiceProvider.RegisterServiceLocator(new AutofacLocator(config));
            ServiceProvider.Instance.Register();

            XSetting.Completed();
        }
    }
}
