using Common.Helpers;
using GalaSoft.MvvmLight.Command;

namespace Presentaion.Desktop.WPF.ViewModels.Common
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
    public class FooterViewModel : BaseViewModel
    {
        #region Properties

        #endregion

        #region Commands
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
        public FooterViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {

            }
        }

        #region Actions
        private void LoadedAction()
        {
            LoggerHelper.Write();
        }

        #endregion
    }
}