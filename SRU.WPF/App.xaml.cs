using Hardcodet.Wpf.TaskbarNotification;
using Sru.Core;
using Sru.Wpf.Input;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Sru.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;
        private HotKey _hotkey;

        protected override void OnStartup(StartupEventArgs eArgs)
        {
            base.OnStartup(eArgs);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            OptionsWindow window = new OptionsWindow();
            var helper = new WindowInteropHelper(window);
            helper.EnsureHandle();
            _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.Z, window);
            _hotkey.HotKeyPressed += HotKeyPressed;
        }

        private void HotKeyPressed(HotKey obj)
        {
            VolumeManager.EjectVolumeDevice("D:");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
