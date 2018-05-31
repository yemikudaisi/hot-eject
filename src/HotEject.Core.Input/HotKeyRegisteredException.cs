using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Core.Input
{
    public class HotKeyRegisteredException : Exception
    {
        public HotKeyRegisteredException()
            : this("HotKey already in use")
        {

        }
        public HotKeyRegisteredException(String message)
            :base(message)
        {

        }
    }
}
