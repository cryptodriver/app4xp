using Application;
using Common.Commands;
using Common.Helpers;
using Common.Models;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace Presentaion.Desktop.WPF.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region property
        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; RaisePropertyChanged(); }
        }
        #endregion

        #region
        private RelayCommand _loadedCommand;
        public RelayCommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() => LoadedAction());
                }
                return _loadedCommand;
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

        #region Actions
        private async void LoadedAction()
        {
            LoggerHelper.Write();

            await Task.Run(async () =>
            {
                var cmd = ServiceProvider.Instance.Get<IGetImageCommand>();

                TRequest req = new TRequest();

                var res = await cmd.Execute(req);

                // Processed successfully
                if (res.Code == Status.OK)
                {
                    Url = res.Body.message;
                }
            });
        }
        #endregion
    }
}