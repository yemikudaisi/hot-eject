using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HotEject.Wpf.Controls
{
    public class OptionsWindow : Window
    {

        static OptionsWindow()
        {
            // initialize a lookless ui
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OptionsWindow),
                new FrameworkPropertyMetadata(typeof(OptionsWindow)));
        }

        public OptionsWindow()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            ShowActivated = false;
            Loaded += ToastWindowLoaded;
            Application.Current.Deactivated += (s,e) => { Close(); };
            //Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => { SetLocation(); }));
        }

        private void SetLocation()
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;// - 40
        }
        private void ToastWindowLoaded(object sender, object e)
        {
            SetLocation();
        }
    }
}
