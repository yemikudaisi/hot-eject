using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbEject
{
    public static class Win32Constants
    {
        // Win32 constants
        public const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        public const int DBT_DEVTYP_HANDLE = 6;
        public const int BROADCAST_QUERY_DENY = 0x424D5144;
        public const int WM_DEVICECHANGE = 0x0219;
        public const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;   // Preparing to remove (any program can disable the removal)
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // removed 
        public const int DBT_DEVTYP_VOLUME = 0x00000002; // drive type is logical volume
    }
}
