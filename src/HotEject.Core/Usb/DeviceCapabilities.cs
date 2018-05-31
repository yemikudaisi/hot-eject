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
    /// Contains constants for determining devices capabilities.
    /// This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum DeviceCapabilities
    {
        Unknown = 0x00000000,
        // matches cfmgr32.h CM_DEVCAP_* definitions

        LockSupported = 0x00000001,
        EjectSupported = 0x00000002,
        Removable = 0x00000004,
        DockDevice = 0x00000008,
        UniqueId = 0x00000010,
        SilentInstall =0x00000020,
        RawDeviceOk = 0x00000040,
        SurpriseRemovalOk = 0x00000080,
        HardwareDisabled = 0x00000100,
        NonDynamic = 0x00000200,
    }
}
