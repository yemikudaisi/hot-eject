using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf.Controls
{
    public interface ITaskbarIconShell
    {
        TaskbarIcon TaskbarIcon { get; set; }
    }
}
