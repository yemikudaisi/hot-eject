using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotEject.Wpf.Controls
{
    /// <summary>
    /// A window manager that hides the MainWindow by default
    /// Design so to enable notification icon appear on startup
    /// </summary>
    [Export]
    public class TaskbarIconWindowManager : WindowManager
    {
        public TaskbarIconWindow MainWindow(object rootModel, object context = null)
        {
            var w = CreateWindow(rootModel, false, context, null); ;
            return w as TaskbarIconWindow;
        }
    }
}
