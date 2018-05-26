using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sru.Wpf.Controls
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
            Loaded += ToastWindowLoaded;
            Deactivated += (s, e) => { Hide(); };
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            ShowActivated = false;

        }

        private void ToastWindowLoaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
