using Common;

using MahApps.Metro.Controls;
using System;
using System.Windows.Forms;

namespace Presentaion.Desktop.WPF.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : MetroWindow
    {
        NotifyIcon _NI;

        public MainView()
        {
            InitializeComponent();

            _NI = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("App.ico"),
                Visible = true
            };
            _NI.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    WindowState = System.Windows.WindowState.Normal;
                };

            ContextMenu _CM = new ContextMenu();

            MenuItem _MI = new MenuItem() { Index = 0, Text = XSetting.Strings["views.common.menu.exit"] };
            _MI.Click += new EventHandler(OnCloseClicked);

            _CM.MenuItems.AddRange(new MenuItem[] { _MI });

            _NI.ContextMenu = _CM;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void OnCloseClicked(object Sender, EventArgs e)
        {
            _NI.Dispose();

            // Clean memory or do something else, but not for now.

            System.Windows.Application.Current.Shutdown();
        }
    }
}
