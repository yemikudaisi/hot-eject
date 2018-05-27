using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Core.Usb
{
    public class USBDeviceInformation
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string DeviceID { get; set; }
        public string PnPDeviceID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public List<string> DriveLetters
        {
            get
            {
                return driveLetters;
            }

            set
            {
                driveLetters = value;
            }
        }

        private List<string> driveLetters;

        public string DriveLettersToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < DriveLetters.Count; i++)
            {
                string letter = DriveLetters[i];
                sb.Append(letter);
                if (i != DriveLetters.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            string driveLetters = DriveLettersToString();
            return String.Format("Name:{0} DeviceID:{1} PnPDeviceID:{2} Description:{3} Status:{4} Caption:{5} DriveLetters:{6}", Name, DeviceID, PnPDeviceID, Description, Status, Caption, driveLetters);
        }
    }
}
