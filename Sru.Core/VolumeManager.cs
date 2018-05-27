using Sru.Core.Usb;
using System;
using System.Collections.Generic;
// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
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

using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Core
{
    public class VolumeManager
    {
        public static void EjectVolumeDevice(String driveLabel)
        {
            VolumeDeviceClass volumes = new VolumeDeviceClass();
            foreach (Volume vol in volumes.Devices.OfType<Volume>())
            {
                if (vol.LogicalDrive.Equals(driveLabel))
                {
                    vol.Eject(false);
                    break;
                }
            }
        }

        public static IList<Volume> EjectRemovableDevices()
        {
            IList<Volume> ejectedDevices = new List<Volume>();
            VolumeDeviceClass volumes = new VolumeDeviceClass();
            foreach (Volume vol in volumes.Devices.OfType<Volume>())
            {
                if (vol.IsUsb && vol.RemovableDevices.Count > 0)
                {
                    vol.Eject(false);
                    // add to list of ejected devices
                    ejectedDevices.Add(vol);
                }
            }
            return ejectedDevices;
        }

        public static VolumeDeviceClass GetDeviceClass()
        {
            return new VolumeDeviceClass();
        }

        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;
        const int FILE_SHARE_READ = 0x1;
        const int FILE_SHARE_WRITE = 0x2;
        const int FSCTL_LOCK_VOLUME = 0x00090018;
        const int FSCTL_DISMOUNT_VOLUME = 0x00090020;
        const int IOCTL_STORAGE_EJECT_MEDIA = 0x2D4808;
        const int IOCTL_STORAGE_MEDIA_REMOVAL = 0x002D4804;

        private VolumeManager() { }

        public static bool EjectDrive(USBDeviceInformation drive)
        {
            if (drive.DriveLetters.Count <= 0)
            {
                return false;
            }
            return EjectDriveLetter(drive.DriveLetters[0]);
        }

        /*
         * driveLetter eg. "H:"
         */
        private static bool EjectDriveLetter(string driveLetter)
        {
            /*
             https://stackoverflow.com/questions/3918248/how-to-eject-a-usb-removable-disk-volume-similar-to-the-eject-function-in-win
             1. obtain a handle to the volume (CreateFile)
             2. try to lock the volume (FSCTL_LOCK_VOLUME)
             3. try to dismount it (FSCTL_DISMOUNT_VOLUME)
             4. disable the prevent storage media removal (IOCTL_STORAGE_MEDIA_REMOVAL)
             5. eject media (IOCTL_STORAGE_EJECT_MEDIA) 
             */

            var volumeHandle = DriveFileHandle(driveLetter);

            if (LockVolume(volumeHandle) == false)
            {
                return false;
            }

            if (DismountVolume(volumeHandle) == false)
            {
                return false;
            }

            if (PreventVolumeRemoval(volumeHandle, false) == false)
            {
                return false;
            }

            if (EjectVolume(volumeHandle) == false)
            {
                return false;
            }

            if (CloseHandle(volumeHandle) == false)
            {
                return false;
            }

            return true;
        }

        private static IntPtr DriveFileHandle(string driveLetter)
        {
            string filename = string.Format(@"\\.\{0}", driveLetter);
            return OSDelegate.CreateFile(filename, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, 0x3, 0, IntPtr.Zero);
        }

        private static bool LockVolume(IntPtr handle)
        {
            uint byteReturned;
            return OSDelegate.DeviceIoControl(handle, FSCTL_LOCK_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);
        }

        private static bool DismountVolume(IntPtr handle)
        {
            uint byteReturned;
            return OSDelegate.DeviceIoControl(handle, FSCTL_DISMOUNT_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);
        }

        private static bool PreventVolumeRemoval(IntPtr handle, bool prevent)
        {
            byte[] buf = new byte[1];
            uint retVal;
            buf[0] = prevent ? (byte)1 : (byte)0;
            return OSDelegate.DeviceIoControl(handle, IOCTL_STORAGE_MEDIA_REMOVAL, buf, 1, IntPtr.Zero, 0, out retVal, IntPtr.Zero);
        }

        private static bool EjectVolume(IntPtr handle)
        {
            uint byteReturned;
            return OSDelegate.DeviceIoControl(handle, IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);
        }

        private static bool CloseHandle(IntPtr handle)
        {
            return OSDelegate.CloseHandle(handle);
        }

        public static List<USBDeviceInformation> ListUSBDevices()
        {
            List<USBDeviceInformation> devices = new List<USBDeviceInformation>();
            ManagementObjectCollection col;
            //string query = @"select * from Win32_USBControllerDevice";
            //string query = @"select * from Win32_USBHub";
            string query = @"select * from Win32_DiskDrive where InterfaceType = 'USB'";
            using (var searcher = new ManagementObjectSearcher(query))
            {
                col = searcher.Get();
            }

            foreach (var device in col)
            {
                // query for partitions
                string deviceID = device.Properties["DeviceID"].Value.ToString();

                var deviceIDs = GetPartitionsDeviceIDs(deviceID);
                List<string> allLetters = new List<string>();
                foreach (var partitionDeviceID in deviceIDs)
                {
                    var letters = GetParitionDriveLetters(partitionDeviceID);
                    foreach (string letter in letters)
                    {
                        allLetters.Add(letter);
                    }
                }

                var deviceInfo = new USBDeviceInformation
                {
                    Name = device.Properties["Name"].Value.ToString(),
                    Caption = device.Properties["Caption"].Value.ToString(),
                    DeviceID = deviceID,
                    PnPDeviceID = device.Properties["PNPDeviceID"].Value.ToString(),
                    Description = device.Properties["Description"].Value.ToString(),
                    Status = device.Properties["Status"].Value.ToString(),
                    DriveLetters = allLetters
                };

                devices.Add(deviceInfo);
            }
            col.Dispose();
            return devices;
        }

        static List<string> GetPartitionsDeviceIDs(string deviceID)
        {
            ManagementObjectCollection partitions;
            List<string> deviceIDs = new List<string>();
            string query = string.Format("associators of {{Win32_DiskDrive.DeviceID='{0}'}} where AssocClass = Win32_DiskDriveToDiskPartition", deviceID);
            using (var searcher = new ManagementObjectSearcher(query))
            {
                partitions = searcher.Get();
            }

            foreach (var parition in partitions)
            {
                deviceIDs.Add(parition.Properties["DeviceID"].Value.ToString());
            }

            partitions.Dispose();
            return deviceIDs;
        }

        static List<string> GetParitionDriveLetters(string partitionDeviceID)
        {
            ManagementObjectCollection disks;
            List<string> letters = new List<string>();
            string query = string.Format("associators of {{Win32_DiskPartition.DeviceID='{0}'}} where AssocClass = Win32_LogicalDiskToPartition", partitionDeviceID);
            using (var searcher = new ManagementObjectSearcher(query))
            {
                disks = searcher.Get();
            }

            foreach (var disk in disks)
            {
                letters.Add(disk.Properties["Name"].Value.ToString());
            }

            disks.Dispose();
            return letters;
        }
    }
}
