# SRU (short for safely remove USB)
## Overview
SRU Utility software that allows user to assign hotkeys (e.g Ctrl + Alt + Z) for use in safely removing USB drives. It was inspired by the majority of people I see around clicking on Ctrl + Alt when attempting to remove USB. I sincerely don't know where the got idea came from. This is me trying to help.

## System Requirements (running SRU)
 - Windows Vista or higher.
 - [.NET 4.5](http://www.microsoft.com/en-au/download/details.aspx?id=30653)
 
 ## Todos
 - Show locking process when file in USB is being used.
 - Configurable hot key (globally and for each drive..not sure how to go about it yet) for (uses Ctrl + Alt + Z for now).
 - Confgure autorun app on device added or removed (handy for backup and sync tools)
 - Add command line support
 - Reconnect a device without removing and re-adding
 - Implement options UI
 - Think of more todos.
 
 
 ## Libraries and Integrated code:

* [Caliburn.micro](https://caliburnmicro.com/)
* [NotifyIcon WPF](https://bitbucket.org/hardcodet/notifyicon-wpf/src)
* [Apache log4net™](https://logging.apache.org/log4net/)
* [Eject USB disks using C#](https://www.codeproject.com/Articles/13530/Eject-USB-disks-using-C)

## Reusable Librarie(s) (Part of SRU):

* [Sru.Core](https://github.com/yemikudaisi/safely-remove-usb/tree/master/Sru.Core)

## SRU Contributors

* [Yemi Kudaisi](https://github.com/yemikudaisi/) (Project Founder)

###### Copyright 2018 Yemi Kudaisi for the SRU. SRU is distributed under the MIT license.
