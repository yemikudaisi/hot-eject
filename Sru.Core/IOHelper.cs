using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbEject
{
    class IOHelper
    {

        /// <summary>
        /// Searches for any file in a given path and returns its full path
        /// </summary>
        /// <param name="drive">drive to search</param>
        /// <returns>path of the file or empty string</returns>
        public  string GetAnyFile(string drive)
        {
            string file = "";
            // First try files in the root
            string[] files = Directory.GetFiles(drive);
            if (files.Length == 0)
            {
                // if no file in the root, search whole drive
                files = Directory.GetFiles(drive, "*.*", SearchOption.AllDirectories);
            }

            if (files.Length > 0)
                file = files[0];        // get the first file

            // return empty string if no file found
            return file;
        }
    }
}
