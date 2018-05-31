using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Wpf.Infrastructure
{
    class Utilities
    {
        static Settings _settings = new Settings();
        public static Settings Settings
        {
            get
            {
                return _settings;
            }
        }
    }
}
