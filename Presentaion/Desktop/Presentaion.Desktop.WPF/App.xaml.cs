using Application;
using Application.Helpers;
using Common;
using Common.Helpers;
using Presentaion.Desktop.WPF.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace Presentaion.Desktop.WPF
{
    public partial class App : System.Windows.Application
    {
        private Notifier _notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new PrimaryScreenPositionProvider(
                corner: Corner.BottomRight, offsetX: 10, offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(5),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
        });

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Process proc = Process.GetCurrentProcess();
            if (Process.GetProcesses().Where(p => p.ProcessName == proc.ProcessName).Count() > 1)
            {
                App.Current.Shutdown();
            }

            Task.Factory.StartNew(() =>
            {
                AssemblyName app = Assembly.GetExecutingAssembly().GetName();

                Bootstrapper.Initialize(
                    new Config
                    {
                        CurrentUI = UIType.WPF,
                        UserAgent = $"{app.Name} {app.Version.Major}.{app.Version.Minor}.{app.Version.Build}.{app.Version.Revision}",
                        DeviceName = Environment.MachineName
                    }
                );

                this.Dispatcher.Invoke(() =>
                {
                    new MainView().Show();
                });
            });
        }

        public App()
        {
            OnUnhandledException();
        }

        private void OnUnhandledException()
        {
            DispatcherUnhandledException +=
                (sender, args) =>
                {
                    args.Handled = true;

                    LoggerHelper.Write(args.Exception.ToString());
                }
            ;

            TaskScheduler.UnobservedTaskException +=
                (sender, args) =>
                {
                    LoggerHelper.Write(args.ToString());
                }
            ;

            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) =>
                {
                    LoggerHelper.Write(args.ToString());
                }
            ;
        }
    }
}
