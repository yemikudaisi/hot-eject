using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf
{
    public class LogManager
    {
        public static log4net.ILog Logger
        {
            get { return log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); }
        }
    }
}
