// Modified from CODEPROJECT article posted on 22 Mar 2006
// https://www.codeproject.com/Articles/13530/Eject-USB-disks-using-C
// Originally written by Simon Mourier <email: simon_mourier@hotmail.com>

using System;
using System.Collections.Generic;
using System.Text;

namespace Sru.Core.Usb
{
    /// <summary>
    /// The device class for disk devices.
    /// </summary>
    public class DiskDeviceClass : DeviceClass
    {
        /// <summary>
        /// Initializes a new instance of the DiskDeviceClass class.
        /// </summary>
        public DiskDeviceClass()
            :base(new Guid(Native.GUID_DEVINTERFACE_DISK))
        {
        }
    }
}
