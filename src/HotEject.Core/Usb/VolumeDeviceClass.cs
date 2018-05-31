// Copyright (c) 2018 Yemi Kudaisi for the SRU
// 
// Modified from CODEPROJECT article posted on 22 Mar 2006
// https://www.codeproject.com/Articles/13530/Eject-USB-disks-using-C

// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;

namespace HotEject.Core.Usb
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
