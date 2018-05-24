// Modified from CODEPROJECT article posted on 22 Mar 2006
// https://www.codeproject.com/Articles/13530/Eject-USB-disks-using-C
// Originally written by Simon Mourier <email: simon_mourier@hotmail.com>

using System;
using System.Collections.Generic;
using System.Text;

namespace Sru.Core.Usb
{
    /// <summary>
    /// The device class for volume devices.
    /// </summary>
    public class VolumeDeviceClass : DeviceClass
    {
        internal SortedDictionary<string, string> _logicalDrives = new SortedDictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the VolumeDeviceClass class.
        /// </summary>
        public VolumeDeviceClass()
            : base(new Guid(Native.GUID_DEVINTERFACE_VOLUME))
        {
            foreach(string drive in Environment.GetLogicalDrives())
            {
                StringBuilder sb = new StringBuilder(1024);
                if (Native.GetVolumeNameForVolumeMountPoint(drive, sb, sb.Capacity))
                {
                    _logicalDrives[sb.ToString()] = drive.Replace("\\", "");
                    Console.WriteLine(drive + " ==> " + sb.ToString());
                }
            }
        }

        internal override Device CreateDevice(DeviceClass deviceClass, Native.SP_DEVINFO_DATA deviceInfoData, string path, int index, int disknum = -1)
        {
            return new Volume(deviceClass, deviceInfoData, path, index);
        }
    }
}
