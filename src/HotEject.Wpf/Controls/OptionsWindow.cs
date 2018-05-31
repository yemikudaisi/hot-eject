using System;
using System.Windows;

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
            Application.Current.Deactivated += ApplicationDeactivated;
        }

        private void ApplicationDeactivated(object sender, EventArgs e)
        {
            Hide();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            Hide();
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
