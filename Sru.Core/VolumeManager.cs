using Sru.Core.Usb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Core
{
    public static class VolumeManager
    {
        public static void EjectVolumeDevice(String driveLabel)
        {
            VolumeDeviceClass volumes = new VolumeDeviceClass();
            foreach (Volume vol in volumes.Devices)
            {
                if (vol.LogicalDrive.Equals(driveLabel))
                {
                    vol.Eject(false);
                    break;
                }
            }
        }
    }
}
