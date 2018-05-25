using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sru.Wpf.Infrastructure
{
    /// <summary>
    /// A window manager that hides the MainWindow by default
    /// Design so to enable notification icon appear on startup
    /// </summary>
    [Export]
    public class CustomWindowManager : WindowManager
    {
        public Window MainWindow(object rootModel, object context = null)
        {
            return CreateWindow(rootModel, false, context, null);
        }
    }
}
