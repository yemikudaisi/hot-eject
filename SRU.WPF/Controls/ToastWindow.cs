using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Sru.Wpf.Controls
{
    public class ToastWindow : OptionsWindow
    {

        // Dependency property backing variables
        public static readonly DependencyProperty TextProperty;

        static ToastWindow()
        {
            // initialize a lookless ui
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToastWindow),
                new FrameworkPropertyMetadata(typeof(ToastWindow)));

            TextProperty = DependencyProperty.Register("Text", typeof(string),
               typeof(ToastWindow), new UIPropertyMetadata(null));
        }

        DispatcherTimer timer = null;

        public ToastWindow()
        {
            Topmost = true;
            Loaded += ToastWindow_Loaded;
            Deactivated += ToastWindowDeactivated;
            Closing += ToastWindowClosing;
            StartTimer();
            this.Opacity = 0;
        }

        private void ToastWindowDeactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void ToastWindowClosing(object sender, CancelEventArgs e)
        {
            Closing -= ToastWindowClosing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.5));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += new EventHandler(timer_Elapsed);
            timer.Start();
        }


        void timer_Elapsed(object sender, EventArgs e)
        {
            timer.Stop();
            Close();
        }

        private void ToastWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - 10;
            this.Top = desktopWorkingArea.Bottom - this.Height - 10;
            Closing -= ToastWindowClosing;
            ;
            var anim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(0.5));
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        /// <summary>
        /// The text displayed by the button.
        /// </summary>
        [Description("The text displayed by the button."), Category("Common Properties")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
