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
    }
}
