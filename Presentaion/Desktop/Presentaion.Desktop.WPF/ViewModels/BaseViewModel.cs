using Common;
using GalaSoft.MvvmLight;
using I18NPortable;
using MahApps.Metro.Controls.Dialogs;
using MvvmValidation;
using System;
using System.Collections;
using System.ComponentModel;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace Presentaion.Desktop.WPF.ViewModels
{
    public class BaseViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public II18N Strings => XSetting.Strings;

        protected ValidationHelper Validator { get; private set; }

        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; set; }

        protected IDialogCoordinator _dialogCoordinator;

        public Notifier Notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                  parentWindow: App.Current.MainWindow,
                  corner: Corner.BottomRight,
                  offsetX: 10,
                  offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                  notificationLifetime: TimeSpan.FromSeconds(3),
                  maximumNotificationCount: MaximumNotificationCount.FromCount(3));

            cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
        });

        public BaseViewModel()
        {
            Validator = new ValidationHelper();

            NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
        }

        public bool HasErrors
        {
            get { return NotifyDataErrorInfoAdapter.HasErrors; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { NotifyDataErrorInfoAdapter.ErrorsChanged += value; }
            remove { NotifyDataErrorInfoAdapter.ErrorsChanged -= value; }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return NotifyDataErrorInfoAdapter.GetErrors(propertyName);
        }
    }
}