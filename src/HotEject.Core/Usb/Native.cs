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
using System.Runtime.InteropServices;
using System.Text;

namespace HotEject.Core.Usb
{
	internal sealed class Native
	{
        // from winuser.h
        internal const int WM_DEVICECHANGE = 0x0219;

		// from winbase.h
		internal const int INVALID_HANDLE_VALUE = -1;
        internal const int GENERIC_READ = unchecked((int)0x80000000);
        internal const int FILE_SHARE_READ = 0x00000001;
        internal const int FILE_SHARE_WRITE = 0x00000002;
        internal const int OPEN_EXISTING = 3;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetVolumeNameForVolumeMountPoint(
            string volumeName,
            StringBuilder uniqueVolumeName,
            int uniqueNameBufferCapacity);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern IntPtr CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern bool DeviceIoControl(IntPtr hDevice, int dwIoControlCode, IntPtr lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hObject);

		// from winerror.h
		internal const int ERROR_NO_MORE_ITEMS = 259;
		internal const int ERROR_INSUFFICIENT_BUFFER = 122;
		internal const int ERROR_INVALID_DATA = 13;

		// from winioctl.h
		internal const string GUID_DEVINTERFACE_VOLUME = "53f5630d-b6bf-11d0-94f2-00a0c91efb8b";
		internal const string GUID_DEVINTERFACE_DISK = "53f56307-b6bf-11d0-94f2-00a0c91efb8b";
        internal const int IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x00560000;
        internal const int IOCTL_STORAGE_GET_DEVICE_NUMBER      = 0x002d1080;
       
        [StructLayout(LayoutKind.Sequential)]
		internal struct DISK_EXTENT 
		{
			internal int DiskNumber;
			internal long StartingOffset;
			internal long ExtentLength;
		}

        [StructLayout(LayoutKind.Sequential)]
        internal struct STORAGE_DEVICE_NUMBER
        {
            public int DeviceType;
            public int DeviceNumber;
            public int PartitionNumber;
        }

        // from cfg.h
        internal enum PNP_VETO_TYPE
        {
            Ok,

            TypeUnknown,
            LegacyDevice,
            PendingClose,
            WindowsApp,
            WindowsService,
            OutstandingOpen,
            Device,
            Driver,
            IllegalDeviceRequest,
            InsufficientPower,
            NonDisableable,
            LegacyDriver,
        }

        // from cfgmgr32.h
        [DllImport("setupapi.dll")]
        internal static extern int CM_Get_Parent(
            ref int pdnDevInst,
            int dnDevInst,
            int ulFlags);

        [DllImport("setupapi.dll")]
        internal static extern int CM_Get_Device_ID(
            int dnDevInst,
            StringBuilder buffer,
            int bufferLen,
            int ulFlags);

        [DllImport("setupapi.dll")]
        internal static extern int CM_Request_Device_Eject(
            int dnDevInst,
            out PNP_VETO_TYPE pVetoType,
            StringBuilder pszVetoName,
            int ulNameLength,
            int ulFlags
            );

        [DllImport("setupapi.dll", EntryPoint = "CM_Request_Device_Eject")]
        internal static extern int CM_Request_Device_Eject_NoUi(
            int dnDevInst,
            IntPtr pVetoType,
            StringBuilder pszVetoName,
            int ulNameLength,
            int ulFlags
            );

        // from setupapi.h
        internal const int DIGCF_PRESENT = (0x00000002);
        internal const int DIGCF_DEVICEINTERFACE = (0x00000010);

        internal const int SPDRP_DEVICEDESC = 0x00000000;
        internal const int SPDRP_CAPABILITIES = 0x0000000F;
        internal const int SPDRP_CLASS = 0x00000007;
        internal const int SPDRP_CLASSGUID = 0x00000008;
        internal const int SPDRP_FRIENDLYNAME = 0x0000000C;

        [StructLayout(LayoutKind.Sequential)]
		internal class SP_DEVINFO_DATA
		{
			internal int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
			internal Guid classGuid = Guid.Empty; // temp
			internal int devInst = 0; // dumy
			internal int reserved = 0;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			internal int cbSize;
            internal short devicePath;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class SP_DEVICE_INTERFACE_DATA 
		{
			internal int cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
			internal Guid interfaceClassGuid = Guid.Empty; // temp
			internal int flags = 0;
			internal int reserved = 0;
		}

        [DllImport("setupapi.dll")]
		internal static extern IntPtr SetupDiGetClassDevs(
			ref Guid classGuid,
			int enumerator,
			IntPtr hwndParent,
			int flags);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr deviceInfoSet,
            SP_DEVINFO_DATA deviceInfoData,
            ref Guid interfaceClassGuid,
            int memberIndex,
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiOpenDeviceInfo(
            IntPtr deviceInfoSet,
            string deviceInstanceId,
            IntPtr hwndParent,
            int openFlags,
            SP_DEVINFO_DATA deviceInfoData
            );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr deviceInfoSet,
            SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            int deviceInterfaceDetailDataSize,
            ref int requiredSize,
            SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(
            IntPtr deviceInfoSet,
            SP_DEVINFO_DATA deviceInfoData,
            int property,
            out int propertyRegDataType,
            IntPtr propertyBuffer,
            int propertyBufferSize,
            out int requiredSize
            );
        
        [DllImport("setupapi.dll")]
		internal static extern uint SetupDiDestroyDeviceInfoList(
			IntPtr deviceInfoSet);


        private Native()
		{
		}
	}
}
