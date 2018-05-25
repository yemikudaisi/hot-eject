using Caliburn.Micro;
using Sru.Wpf.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf
{
    [Export(typeof(IShell))]
    public class TrayIconViewModel : Screen, IShell
    {
        public void AppExit()
        {
            Environment.Exit(0);
        }
    }
}
