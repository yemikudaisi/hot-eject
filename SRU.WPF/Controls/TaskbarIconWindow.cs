using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sru.Wpf.Controls
{
    public abstract class TaskbarIconWindow : Window
    {
        public abstract TaskbarIcon TaskBarIcon { get; }
    }
}
